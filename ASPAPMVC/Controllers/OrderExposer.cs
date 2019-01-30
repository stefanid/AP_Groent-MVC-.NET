using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Controllers
{
    public class OrderExposer
    {
        private List<Order> _orderList;
        private List<Order> _recievedOrderList;
        private Order _order;
        private System.Configuration.Configuration rootWebConfig;
        private System.Configuration.ConnectionStringSettings connString;
        private DatabaseComponent.DatabaseAccessLayer _dbAccessLayer;

        public OrderExposer()
        {
            _orderList = new List<Order>();
            _recievedOrderList = new List<Order>();
            _order = new Order();
            rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/ASPAPMVC");
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["APDEV"];
            }
            _dbAccessLayer = new DatabaseComponent.DatabaseAccessLayer(connString.ToString());
        }

        public List<Order> getTodaysOrder()
        {
            var sQuery = "SELECT *, (Select Supplier_Name from Supplier where Supplier_UID = iOrder.Supplier_UID) AS Supplier_Name FROM iOrder" +
                " WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, iOrder.iOrder_CreateDate)) = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";

            DataSet _todaysOrders = _dbAccessLayer.runSQLDataSet(sQuery);

            for (int i = 0; i < _todaysOrders.Tables[0].Rows.Count; i++)
            {
                Order order = new Order();
                order.iOrder_UID = Guid.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_UID"].ToString());
                order.Supplier_Name = _todaysOrders.Tables[0].Rows[i]["Supplier_Name"].ToString();
                order.iOrder_ID = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_ID"].ToString());
                order.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_StatusID"].ToString());
                order.iOrder_SupplierRef = _todaysOrders.Tables[0].Rows[i]["iOrder_SupplierRef"].ToString();

                order.iOrder_iOrderDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_iOrderDate"].ToString());
                var sOrderDate = order.iOrder_iOrderDate.ToString();
                DateTime datetimeOrder = DateTime.ParseExact(sOrderDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringOrderDate = datetimeOrder.ToString("yyyy/MM/dd");

                order.iOrder_DeliveryDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_DeliveryDate"].ToString());
                var sDevDate = order.iOrder_DeliveryDate.ToString();
                DateTime datetimeDeliver = DateTime.ParseExact(sDevDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringDeliveryDate = datetimeDeliver.ToString("yyyy/MM/dd");


                order.iOrder_CreateDate = DateTime.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_CreateDate"].ToString());
                order.StringCreateDate = order.iOrder_CreateDate.ToString("yyyy/MM/dd");
                order.iOrder_Organic = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_Organic"].ToString());
                switch (order.iOrder_Organic)
                {
                    case 1:
                        order.StringifyOrganic = "Yes";
                        break;
                    case 0:
                        order.StringifyOrganic = "No";
                        break;
                    default:
                        order.StringifyOrganic = "Not set";
                        break;
                }
                order.ShipStatus = (ShippingStatus)Enum.Parse(typeof(ShippingStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_ConfirmationShipped"].ToString());
                _orderList.Add(order);



            }
            return _orderList;
        }

        public Order AddUpdateOrder(Order order)
        {
            order.iOrder_iOrderDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            order.iOrder_StatusID = 10;
            string sp = "SP_iOrder_AddUpdate";
            _dbAccessLayer.AddParameter("@iUID", order.iOrder_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@iID", order.iOrder_ID, SqlDbType.BigInt);
            _dbAccessLayer.AddParameter("@SupplierUID", order.Supplier_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@SupplierRef", order.iOrder_SupplierRef, SqlDbType.NVarChar);
            _dbAccessLayer.AddParameter("@iOrderDate", order.iOrder_iOrderDate, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@DeliveryDate", order.iOrder_DeliveryDate, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iStatusID", order.iOrder_StatusID, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@Organic", order.iOrder_Organic, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iconfirmationShip", order.iOrder_ConfirmationShipped, SqlDbType.Int);
            DataSet addedOrder = _dbAccessLayer.runSPDataSet(sp);
            if (addedOrder.Tables[0].Rows.Count > 0)
            {
                _order.iOrder_UID = Guid.Parse(addedOrder.Tables[0].Rows[0]["iOrder_UID"].ToString());

            }
            return _order;
        }

        public Order AddUpdateRecievedOrder(Order order)
        {
            order.iOrder_iOrderDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            order.iOrder_StatusID = 20;
            string sp = "SP_iOrder_AddUpdate";
            _dbAccessLayer.AddParameter("@iUID", order.iOrder_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@iID", order.iOrder_ID, SqlDbType.BigInt);
            _dbAccessLayer.AddParameter("@SupplierUID", order.Supplier_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@SupplierRef", order.iOrder_SupplierRef, SqlDbType.NVarChar);
            _dbAccessLayer.AddParameter("@iOrderDate", order.iOrder_iOrderDate, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@DeliveryDate", order.iOrder_DeliveryDate, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iStatusID", order.iOrder_StatusID, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@Organic", order.iOrder_Organic, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iconfirmationShip", order.iOrder_ConfirmationShipped, SqlDbType.Int);
            DataSet addedOrder = _dbAccessLayer.runSPDataSet(sp);
            if (addedOrder.Tables[0].Rows.Count > 0)
            {
                _order.iOrder_UID = Guid.Parse(addedOrder.Tables[0].Rows[0]["iOrder_UID"].ToString());

            }
            return _order;
        }

        public Order getOrder(string iOrder_UID)
        {
            Order _foundOrder = new Order();
            var sQuery = "SELECT * FROM iOrder WHERE iOrder_UID = @iOrder_UID";
            _dbAccessLayer.AddParameter("@iOrder_UID", Guid.Parse(iOrder_UID), SqlDbType.UniqueIdentifier);
            DataSet order = _dbAccessLayer.runSQLDataSet(sQuery);
            if (order.Tables[0].Rows.Count > 0)
            {
                _foundOrder.iOrder_UID = Guid.Parse(order.Tables[0].Rows[0]["iOrder_UID"].ToString());
                _foundOrder.ShipStatus = (ShippingStatus)Enum.Parse(typeof(ShippingStatus), order.Tables[0].Rows[0]["iOrder_ConfirmationShipped"].ToString());
                _foundOrder.Supplier_UID = Guid.Parse(order.Tables[0].Rows[0]["Supplier_UID"].ToString());
                _foundOrder.iOrder_ID = Convert.ToInt32(order.Tables[0].Rows[0]["iOrder_ID"].ToString());
                _foundOrder.iOrder_SupplierRef = order.Tables[0].Rows[0]["iOrder_SupplierRef"].ToString();
                _foundOrder.iOrder_DeliveryDate = int.Parse(order.Tables[0].Rows[0]["iOrder_DeliveryDate"].ToString());
                _foundOrder.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), order.Tables[0].Rows[0]["iOrder_StatusID"].ToString());
                DateTime datetimeDeliver = DateTime.ParseExact(_foundOrder.iOrder_DeliveryDate.ToString(), 
                    "yyyyMMdd", CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                string formattedDateTime = datetimeDeliver.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                _foundOrder.StringDeliveryDate = formattedDateTime;
                _foundOrder.iOrder_Organic = int.Parse(order.Tables[0].Rows[0]["iOrder_Organic"].ToString());

            }
            return _foundOrder;
        }

        public bool deleteOrder(string iOrder_UID)
        {
            try
            {
                _order.iOrder_UID = Guid.Parse(iOrder_UID);
                var sQuery = "DELETE FROM iOrder WHERE iOrder_UID = @iOrder_UID";
                _dbAccessLayer.AddParameter("@iOrder_UID", _order.iOrder_UID, SqlDbType.UniqueIdentifier);
                DataSet deleteOrder = _dbAccessLayer.runSQLDataSet(sQuery);
                return true;
            }
            catch 
            {

                return false;
            }
            
        }  

        public List<Order> getRecievedOrderLineWithoutOrderLines()
        {
            var sQuery = "SELECT *, (Select Supplier_Name from Supplier where Supplier_UID = iOrder.Supplier_UID) AS Supplier_Name FROM iOrder" +
                " JOIN iOrderLine ON iOrder.iOrder_UID = iOrderLine.iOrder_UID" +
                " WHERE iOrder.iOrder_StatusID = @iOrder_StatusID AND iOrderLine.iOrderLine_StatusID = @iOrderLine_StatusID" +
                " ORDER BY  iOrder_DeliveryDate ASC;";

            _dbAccessLayer.AddParameter("@iOrder_StatusID", 20, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iOrderLine_StatusID", 20, SqlDbType.Int);
            DataSet _todaysOrders = _dbAccessLayer.runSQLDataSet(sQuery);


            for (int i = 0; i < _todaysOrders.Tables[0].Rows.Count; i++)
            {
                Order order = new Order();
                order.iOrder_UID = Guid.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_UID"].ToString());
                order.Supplier_Name = _todaysOrders.Tables[0].Rows[i]["Supplier_Name"].ToString();
                order.iOrder_ID = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_ID"].ToString());
                order.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_StatusID"].ToString());
                order.iOrder_SupplierRef = _todaysOrders.Tables[0].Rows[i]["iOrder_SupplierRef"].ToString();

                order.iOrder_iOrderDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_iOrderDate"].ToString());
                var sOrderDate = order.iOrder_iOrderDate.ToString();
                DateTime datetimeOrder = DateTime.ParseExact(sOrderDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringOrderDate = datetimeOrder.ToString("yyyy/MM/dd");

                order.iOrder_DeliveryDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_DeliveryDate"].ToString());
                var sDevDate = order.iOrder_DeliveryDate.ToString();
                DateTime datetimeDeliver = DateTime.ParseExact(sDevDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringDeliveryDate = datetimeDeliver.ToString("yyyy/MM/dd");


                order.iOrder_CreateDate = DateTime.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_CreateDate"].ToString());
                order.StringCreateDate = order.iOrder_CreateDate.ToString("yyyy/MM/dd");
                order.iOrder_Organic = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_Organic"].ToString());
                switch (order.iOrder_Organic)
                {
                    case 1:
                        order.StringifyOrganic = "Yes";
                        break;
                    case 0:
                        order.StringifyOrganic = "No";
                        break;
                    default:
                        order.StringifyOrganic = "Not set";
                        break;
                }
                order.ShipStatus = (ShippingStatus)Enum.Parse(typeof(ShippingStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_ConfirmationShipped"].ToString());
                _orderList.Add(order);
                
            }
            return _orderList;
        }


        public List<Order> getRecievedOrderLineWithOrderLines()
        {
            var sQuery = "SELECT *, (Select Supplier_Name from Supplier where Supplier_UID = iOrder.Supplier_UID) AS Supplier_Name FROM iOrder" +
                " JOIN iOrderLine ON iOrder.iOrder_UID = iOrderLine.iOrder_UID" +
                " WHERE iOrder.iOrder_StatusID = @iOrder_StatusID AND iOrderLine.iOrderLine_StatusID = @iOrderLine_StatusID" +
                " ORDER BY  iOrder_DeliveryDate ASC;";

            _dbAccessLayer.AddParameter("@iOrder_StatusID", 20, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@iOrderLine_StatusID", 20, SqlDbType.Int);
            DataSet _todaysOrders = _dbAccessLayer.runSQLDataSet(sQuery);

            for (int i = 0; i < _todaysOrders.Tables[0].Rows.Count; i++)
            {
                Order order = new Order();
                order.iOrder_UID = Guid.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_UID"].ToString());
                order.Supplier_Name = _todaysOrders.Tables[0].Rows[i]["Supplier_Name"].ToString();
                order.iOrder_ID = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_ID"].ToString());
                order.OrderStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_StatusID"].ToString());
                order.iOrder_SupplierRef = _todaysOrders.Tables[0].Rows[i]["iOrder_SupplierRef"].ToString();

                order.iOrder_iOrderDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_iOrderDate"].ToString());
                var sOrderDate = order.iOrder_iOrderDate.ToString();
                DateTime datetimeOrder = DateTime.ParseExact(sOrderDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringOrderDate = datetimeOrder.ToString("yyyy/MM/dd");

                order.iOrder_DeliveryDate = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_DeliveryDate"].ToString());
                var sDevDate = order.iOrder_DeliveryDate.ToString();
                DateTime datetimeDeliver = DateTime.ParseExact(sDevDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                order.StringDeliveryDate = datetimeDeliver.ToString("yyyy/MM/dd");


                order.iOrder_CreateDate = DateTime.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_CreateDate"].ToString());
                order.StringCreateDate = order.iOrder_CreateDate.ToString("yyyy/MM/dd");
                order.iOrder_Organic = int.Parse(_todaysOrders.Tables[0].Rows[i]["iOrder_Organic"].ToString());
                switch (order.iOrder_Organic)
                {
                    case 1:
                        order.StringifyOrganic = "Yes";
                        break;
                    case 0:
                        order.StringifyOrganic = "No";
                        break;
                    default:
                        order.StringifyOrganic = "Not set";
                        break;
                }
                order.ShipStatus = (ShippingStatus)Enum.Parse(typeof(ShippingStatus), _todaysOrders.Tables[0].Rows[i]["iOrder_ConfirmationShipped"].ToString());
                _orderList.Add(order);

            }
            return _orderList;
        }

    }
}