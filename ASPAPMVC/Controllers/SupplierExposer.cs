

using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Controllers
{
    public class SupplierExposer
    {
        private List<Supplier> _suppliersList;
        private List<Country> _countries;
        private Supplier _supplier;
        private System.Configuration.Configuration rootWebConfig;
        private System.Configuration.ConnectionStringSettings connString;
        private DatabaseComponent.DatabaseAccessLayer _dbAccessLayer;

        public SupplierExposer()
        {
            _suppliersList = new List<Supplier>();
            _supplier = new Supplier();
            rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/ASPAPMVC");
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["APDEV"];
            }
            _dbAccessLayer = new DatabaseComponent.DatabaseAccessLayer(connString.ToString());
        }

        public Supplier AddUpdateSupplier(Supplier new_supplier)
        {
            string sp = "SP_Supplier_AddUpdate";


            _dbAccessLayer.AddParameter("@SUID", new_supplier.Supplier_UID, System.Data.SqlDbType.UniqueIdentifier);

            _dbAccessLayer.AddParameter("@SName", new_supplier.Supplier_Name, System.Data.SqlDbType.NVarChar);

            _dbAccessLayer.AddParameter("@SCountryUID", new_supplier.Country_UID, System.Data.SqlDbType.UniqueIdentifier);


            DataSet createdSupplier = _dbAccessLayer.runSPDataSet(sp);
            if (createdSupplier.Tables[0].Rows.Count > 0)
            {
                _supplier.Supplier_UID = Guid.Parse(createdSupplier.Tables[0].Rows[0]["Supplier_UID"].ToString());
                _supplier.Supplier_Name = createdSupplier.Tables[0].Rows[0]["Supplier_Name"].ToString();

            }
            return _supplier;
        }

        public List<Supplier> GetSuppliers()
        {
            var sQuery = "SELECT *, (Select Country_Name from Country where Country_UID = Supplier.Country_UID) as CountryName FROM Supplier";
            DataSet _suppliers = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < _suppliers.Tables[0].Rows.Count; i++)
            {
                Supplier _supplier = new Supplier();
                _supplier.Supplier_UID = Guid.Parse(_suppliers.Tables[0].Rows[i]["Supplier_UID"].ToString());
                _supplier.Country_Name = _suppliers.Tables[0].Rows[i]["CountryName"].ToString();
                _supplier.Supplier_Name = _suppliers.Tables[0].Rows[i]["Supplier_Name"].ToString();
                _supplier.Country_UID = Guid.Parse(_suppliers.Tables[0].Rows[i]["Country_UID"].ToString());
                _suppliersList.Add(_supplier);
            }
            return _suppliersList;
        }

        public List<Country> getCountries()
        {
            _countries = new List<Country>();

            var sQuery = "SELECT * FROM Country";
            DataSet countries = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < countries.Tables[0].Rows.Count; i++)
            {
                Country _country = new Country();
                _country.Country_UID = countries.Tables[0].Rows[i]["Country_UID"].ToString();
                _country.Country_Name = countries.Tables[0].Rows[i]["Country_Name"].ToString();
                _countries.Add(_country);

            }
            return _countries;
        }

        public bool deleteSupplier(Supplier selected_supplier)
        {
            var sQuery = "DELETE FROM Supplier WHERE Supplier_UID = @Supplier_UID";
            _dbAccessLayer.AddParameter("@Supplier_UID", selected_supplier.Supplier_UID, SqlDbType.UniqueIdentifier);
            DataSet dataDeleted = _dbAccessLayer.runSQLDataSet(sQuery);
            return true;


        }

        public Supplier getSupplier(string Supplier_UID)
        {
            Supplier _foundSupplier = new Supplier();
            var sQuery = "SELECT *, (Select Country_Name from Country where Country_UID = Supplier.Country_UID) as CountryName FROM Supplier WHERE Supplier_UID = @Supplier_UID";
            _dbAccessLayer.AddParameter("@Supplier_UID", Guid.Parse(Supplier_UID), SqlDbType.UniqueIdentifier);
            DataSet selectedSupplier = _dbAccessLayer.runSQLDataSet(sQuery);
            if(selectedSupplier.Tables[0].Rows.Count > 0)
            {
                _foundSupplier.Country_UID = Guid.Parse(selectedSupplier.Tables[0].Rows[0]["Country_UID"].ToString());
                _foundSupplier.Supplier_Name = selectedSupplier.Tables[0].Rows[0]["Supplier_Name"].ToString();
                _foundSupplier.Country_Name = selectedSupplier.Tables[0].Rows[0]["CountryName"].ToString();
                return _foundSupplier;
            }
            return _foundSupplier;
        }
    }
}