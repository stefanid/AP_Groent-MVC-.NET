using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Controllers
{
    public class OrderLineExposer
    {
        private List<Supplier> _suppliersList;
        private OrderLine _orderLine;
        private List<Country> _countries;
        private List<Order> _orderList;
        private List<Product_ProductVariation> _productProductVariationList;
        private List<OrderLine> _orderLineList;
        private List<BoxType> _boxTypes;
        private Supplier _supplier;
        private System.Configuration.Configuration rootWebConfig;
        private System.Configuration.ConnectionStringSettings connString;
        private DatabaseComponent.DatabaseAccessLayer _dbAccessLayer;

        public OrderLineExposer()
        {
            _productProductVariationList = new List<Product_ProductVariation>();
            _suppliersList = new List<Supplier>();
            _boxTypes = new List<BoxType>();
            _orderList = new List<Order>();
            _orderLineList = new List<OrderLine>();
            _orderLine = new OrderLine();
            _supplier = new Supplier();
            rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/ASPAPMVC");
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["APDEV"];
            }
            _dbAccessLayer = new DatabaseComponent.DatabaseAccessLayer(connString.ToString());
        }

        public List<OrderLine> getOrderLine(string Order_UID)
        {
            var sQuery = "SELECT ol.iOrderLine_UID,ol.iOrderLine_DeliveryLotNr, o.iOrder_ID, pv.ProductVariation_UID," +
                         " pv.ProductVariation_Name, p.Product_Name, ol.iOrderLine_Pallets, bt.BoxType_Name," +
                         " ol.iOrderLine_OrderNetto, ol.iOrderLine_OrderBrutto" +
                         " FROM iOrderLine AS ol" +
                         " JOIN iOrder AS o" +
                         " ON ol.iOrder_UID = o.iOrder_UID" +
                         " JOIN ProductVariation AS pv" +
                         " ON ol.ProductVariation_UID = pv.ProductVariation_UID" +
                         " JOIN Product AS p" +
                         " ON pv.Product_UID = p.Product_UID" +
                         " JOIN BoxType AS bt" +
                         " ON ol.BoxType_UID = bt.BoxType_UID" +
                         " WHERE ol.iOrder_UID = @iOrder_UID";
            _dbAccessLayer.AddParameter("@iOrder_UID", Guid.Parse(Order_UID), SqlDbType.UniqueIdentifier);
            DataSet _orderLines = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < _orderLines.Tables[0].Rows.Count; i++)
            {
                OrderLine orderLine = new OrderLine();
                orderLine.iOrderLine_UID = Guid.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_UID"].ToString());
                orderLine.iOrderLine_DeliveryLotNr = _orderLines.Tables[0].Rows[i]["iOrderLine_DeliveryLotNr"].ToString();
                orderLine.iOrder_ID = int.Parse(_orderLines.Tables[0].Rows[i]["iOrder_ID"].ToString());
                orderLine.ProductVariation_UID = Guid.Parse(_orderLines.Tables[0].Rows[i]["ProductVariation_UID"].ToString());
                orderLine.ProductVariation_Name = _orderLines.Tables[0].Rows[i]["ProductVariation_Name"].ToString();
                orderLine.Product_Name = _orderLines.Tables[0].Rows[i]["Product_Name"].ToString();
                orderLine.iOrderLine_Pallets = int.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_Pallets"].ToString());
                orderLine.BoxType_Name = _orderLines.Tables[0].Rows[i]["BoxType_Name"].ToString();
                orderLine.iOrderLine_OrderNetto = decimal.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_OrderNetto"].ToString());
                orderLine.iOrderLine_OrderBrutto = decimal.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_OrderBrutto"].ToString());

                _orderLineList.Add(orderLine);
            }
            return _orderLineList;
        }

        public List<OrderLine> getOrderLineOld(string Order_UID)
        {
            var sQuery = "SELECT ol.iOrderLine_UID,ol.iOrderLine_DeliveryLotNr, o.iOrder_ID, pv.ProductVariation_UID," +
                         " pv.ProductVariation_Name, p.Product_Name, ol.iOrderLine_Pallets, bt.BoxType_Name," +
                         " ol.iOrderLine_OrderNetto, ol.iOrderLine_OrderBrutto, ol.iOrderLine_RecivedNetto, ol.iOrderLine_RecivedBrutto," +
                         " ol.iOrderLine_Temperature, ol.iOrderLine_Colli, ol.iOrderLine_ErrSize, ol.iOrderLine_ErrForm, ol.iOrderLine_ErrColor," +
                         " ol.iOrderLine_InsectDamage, ol.iOrderLine_FungiDamage, ol.iOrderLine_PhysicalDamage, ol.iOrderLine_Note" +
                         " FROM iOrderLine AS ol" +
                         " JOIN iOrder AS o" +
                         " ON ol.iOrder_UID = o.iOrder_UID" +
                         " JOIN ProductVariation AS pv" +
                         " ON ol.ProductVariation_UID = pv.ProductVariation_UID" +
                         " JOIN Product AS p" +
                         " ON pv.Product_UID = p.Product_UID" +
                         " JOIN BoxType AS bt" +
                         " ON ol.BoxType_UID = bt.BoxType_UID" +
                         " WHERE ol.iOrder_UID = @iOrder_UID";
            _dbAccessLayer.AddParameter("@iOrder_UID", Guid.Parse(Order_UID), SqlDbType.UniqueIdentifier);
            DataSet _orderLines = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < _orderLines.Tables[0].Rows.Count; i++)
            {
                OrderLine orderLine = new OrderLine();
                orderLine.iOrderLine_UID = Guid.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_UID"].ToString());
                orderLine.iOrderLine_DeliveryLotNr = _orderLines.Tables[0].Rows[i]["iOrderLine_DeliveryLotNr"].ToString();
                orderLine.iOrder_ID = int.Parse(_orderLines.Tables[0].Rows[i]["iOrder_ID"].ToString());
                orderLine.ProductVariation_UID = Guid.Parse(_orderLines.Tables[0].Rows[i]["ProductVariation_UID"].ToString());
                orderLine.ProductVariation_Name = _orderLines.Tables[0].Rows[i]["ProductVariation_Name"].ToString();
                orderLine.Product_Name = _orderLines.Tables[0].Rows[i]["Product_Name"].ToString();
                orderLine.iOrderLine_Pallets = int.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_Pallets"].ToString());
                orderLine.BoxType_Name = _orderLines.Tables[0].Rows[i]["BoxType_Name"].ToString();
                orderLine.iOrderLine_OrderNetto = Convert.ToDecimal(_orderLines.Tables[0].Rows[i]["iOrderLine_OrderNetto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_OrderBrutto = Convert.ToDecimal(_orderLines.Tables[0].Rows[i]["iOrderLine_OrderBrutto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_RecivedNetto = Convert.ToDecimal(_orderLines.Tables[0].Rows[i]["iOrderLine_RecivedNetto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_RecivedBrutto = Convert.ToDecimal(_orderLines.Tables[0].Rows[i]["iOrderLine_RecivedBrutto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_Temperature = Convert.ToDecimal(_orderLines.Tables[0].Rows[i]["iOrderLine_Temperature"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_Colli = int.Parse(_orderLines.Tables[0].Rows[i]["iOrderLine_Colli"].ToString());
                orderLine.iOrderLine_ErrSize = _orderLines.Tables[0].Rows[i]["iOrderLine_ErrSize"].ToString();
                orderLine.iOrderLine_ErrForm = _orderLines.Tables[0].Rows[i]["iOrderLine_ErrForm"].ToString();
                orderLine.iOrderLine_ErrColor = _orderLines.Tables[0].Rows[i]["iOrderLine_ErrColor"].ToString();
                orderLine.iOrderLine_InsectDamage = _orderLines.Tables[0].Rows[i]["iOrderLine_InsectDamage"].ToString();
                orderLine.iOrderLine_FungiDamage = _orderLines.Tables[0].Rows[i]["iOrderLine_FungiDamage"].ToString();
                orderLine.iOrderLine_PhysicalDamage = _orderLines.Tables[0].Rows[i]["iOrderLine_PhysicalDamage"].ToString();
                orderLine.iOrderLine_Note = _orderLines.Tables[0].Rows[i]["iOrderLine_Note"].ToString();
                orderLine.Extra = "Size: " + orderLine.iOrderLine_ErrSize + ";" + "Form: " + orderLine.iOrderLine_ErrForm + ";" + "Color: "
                    + orderLine.iOrderLine_ErrColor + ";" + "Insect Damage: " + orderLine.iOrderLine_InsectDamage + ";" + "Fungi Damage: " +
                    orderLine.iOrderLine_FungiDamage + ";" + "Physical Damage: " + orderLine.iOrderLine_PhysicalDamage + ";" + "Note: " +
                     orderLine.iOrderLine_Note;
                _orderLineList.Add(orderLine);
            }
            return _orderLineList;
        }


        public List<BoxType> getBoxTypes()
        {
            var sQuery = "SELECT * FROM BoxType";
            DataSet boxTypesDT = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < boxTypesDT.Tables[0].Rows.Count; i++)
            {
                BoxType boxType = new BoxType();
                boxType.BoxType_UID = Guid.Parse(boxTypesDT.Tables[0].Rows[i]["BoxType_UID"].ToString());
                boxType.BoxType_Name = boxTypesDT.Tables[0].Rows[i]["BoxType_Name"].ToString();
                _boxTypes.Add(boxType);
            }
            return _boxTypes;

        }

        public List<Product_ProductVariation> getProductProductVar()
        {
            var sQuery = "SELECT pv.ProductVariation_UID," +
                         " pv.ProductVariation_Name, p.Product_Name" +
                         " FROM ProductVariation AS pv" +
                         " JOIN Product AS p" +
                         " ON pv.Product_UID = p.Product_UID";
            DataSet productProductVar = _dbAccessLayer.runSQLDataSet(sQuery);
            for (int i = 0; i < productProductVar.Tables[0].Rows.Count; i++)
            {
                Product_ProductVariation product_ProductVariation = new Product_ProductVariation();
                product_ProductVariation.ProductVariation_Name = productProductVar.Tables[0].Rows[i]["ProductVariation_Name"].ToString();
                product_ProductVariation.Product_Name = productProductVar.Tables[0].Rows[i]["Product_Name"].ToString();
                product_ProductVariation.ProductVariation_UID = Guid.Parse(productProductVar.Tables[0].Rows[i]["ProductVariation_UID"].ToString());

                _productProductVariationList.Add(product_ProductVariation);
            }
            return _productProductVariationList;
        }

        public OrderLine addUpdateOrderLine(OrderLine _orderline)
        {
            var sQuery = "SP_iOrderLine_AddUpdate";
            _dbAccessLayer.AddParameter("@UID", _orderline.iOrderLine_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@OrderUID", _orderline.iOrder_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@DeliveryLotNr", _orderline.iOrderLine_DeliveryLotNr, SqlDbType.NVarChar);
            _dbAccessLayer.AddParameter("@ProductVariationUID", _orderline.ProductVariation_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@BoxType_UID", _orderline.BoxType_UID, SqlDbType.UniqueIdentifier);
            _dbAccessLayer.AddParameter("@OrderNetto", _orderline.iOrderLine_OrderNetto, SqlDbType.Decimal);
            _dbAccessLayer.AddParameter("@OrderBrutto", _orderline.iOrderLine_OrderBrutto, SqlDbType.Decimal);
            _dbAccessLayer.AddParameter("@Pallets", _orderline.iOrderLine_Pallets, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@Organic", _orderline.iOrderLine_Organic, SqlDbType.Int);
            _dbAccessLayer.AddParameter("@StatusID", 10, SqlDbType.Int);
            DataSet _orderLineDT = _dbAccessLayer.runSPDataSet(sQuery);
            if (_orderLineDT.Tables[0].Rows.Count > 0)
            {
                _orderLine.iOrderLine_UID = Guid.Parse(_orderLineDT.Tables[0].Rows[0]["iOrderLine_UID"].ToString());

            }
            return _orderLine;
        }

        public bool deleteOrderLine(string OrderLine_UID)
        {
            try
            {
                var orderline_uid = Guid.Parse(OrderLine_UID);
                var sQuery = "DELETE FROM iOrderLine WHERE iOrderLine_UID = @iOrderLine_UID";
                _dbAccessLayer.AddParameter("@iOrderLine_UID", orderline_uid, SqlDbType.UniqueIdentifier);
                DataSet _deleteOrderLine = _dbAccessLayer.runSQLDataSet(sQuery);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public OrderLine getOrderLineSingle(string iOrderLine_UID)
        {
            OrderLine orderLine = new OrderLine();
            var orderline_uid = Guid.Parse(iOrderLine_UID);
            var sQuery = "SELECT * FROM iOrderLine WHERE iOrderLine_UID = @iOrderLine_UID";
            _dbAccessLayer.AddParameter("@iOrderLine_UID", orderline_uid, SqlDbType.UniqueIdentifier);
            DataSet dataOrder = _dbAccessLayer.runSQLDataSet(sQuery);
            if (dataOrder.Tables[0].Rows.Count > 0)
            {
                orderLine.iOrderLine_OrderBrutto = Convert.ToDecimal(dataOrder.Tables[0].Rows[0]["iOrderLine_OrderBrutto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.iOrderLine_OrderNetto = Convert.ToDecimal(dataOrder.Tables[0].Rows[0]["iOrderLine_OrderNetto"].ToString(), CultureInfo.InvariantCulture);
                orderLine.ProductVariation_UID = Guid.Parse(dataOrder.Tables[0].Rows[0]["ProductVariation_UID"].ToString());
                orderLine.BoxType_UID = Guid.Parse(dataOrder.Tables[0].Rows[0]["BoxType_UID"].ToString());

            }
            return orderLine;

        }
        public OrderLine updateReceiveLine(RecievedOrderLine recievedOrderLine)
        {
            {
                var sQuery = "SP_iOrderLine_AddUpdate";
                _dbAccessLayer.AddParameter("@UID", recievedOrderLine.iOrderLine_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@OrderUID", recievedOrderLine.iOrder_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@DeliveryLotNr", recievedOrderLine.iOrderLine_DeliveryLotNr, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@ProductVariationUID", recievedOrderLine.ProductVariation_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@BoxType_UID", recievedOrderLine.BoxType_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@OrderBrutto", recievedOrderLine.iOrderLine_OrderBrutto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@OrderNetto", recievedOrderLine.iOrderLine_RecivedNetto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@RecivedNetto", recievedOrderLine.iOrderLine_RecivedNetto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@RecivedBrutto", recievedOrderLine.iOrderLine_RecivedBrutto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@Temperature", recievedOrderLine.iOrderLine_Pallets, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@Colli", recievedOrderLine.iOrderLine_Colli, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@Pallets", recievedOrderLine.iOrderLine_Pallets, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@StatusID", 10, SqlDbType.Int);
                DataSet _orderLineDT = _dbAccessLayer.runSPDataSet(sQuery);
                if (_orderLineDT.Tables[0].Rows.Count > 0)
                {
                    _orderLine.iOrderLine_UID = Guid.Parse(_orderLineDT.Tables[0].Rows[0]["iOrderLine_UID"].ToString());

                }
                return _orderLine;
            }
        }

        public OrderLine updateControlReceiveLine(RecievedOrderLine recievedOrderLine)
        {
            {
                var sQuery = "SP_iOrderLine_AddUpdate";
                _dbAccessLayer.AddParameter("@UID", recievedOrderLine.iOrderLine_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@OrderUID", recievedOrderLine.iOrder_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@DeliveryLotNr", recievedOrderLine.iOrderLine_DeliveryLotNr, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@ProductVariationUID", recievedOrderLine.ProductVariation_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@BoxType_UID", recievedOrderLine.BoxType_UID, SqlDbType.UniqueIdentifier);
                _dbAccessLayer.AddParameter("@OrderBrutto", recievedOrderLine.iOrderLine_OrderBrutto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@OrderNetto", recievedOrderLine.iOrderLine_RecivedNetto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@RecivedNetto", recievedOrderLine.iOrderLine_RecivedNetto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@RecivedBrutto", recievedOrderLine.iOrderLine_RecivedBrutto, SqlDbType.Decimal);
                _dbAccessLayer.AddParameter("@Temperature", recievedOrderLine.iOrderLine_Pallets, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@Colli", recievedOrderLine.iOrderLine_Colli, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@Pallets", recievedOrderLine.iOrderLine_Pallets, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@StatusID", 20, SqlDbType.Int);
                _dbAccessLayer.AddParameter("@ErrSize", recievedOrderLine.iOrderLine_ErrSize, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@ErrForm", recievedOrderLine.iOrderLine_ErrForm, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@ErrColor", recievedOrderLine.iOrderLine_ErrColor, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@InsectDamage", recievedOrderLine.iOrderLine_InsectDamage, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@FungiDamage", recievedOrderLine.iOrderLine_InsectDamage, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@PhysicalDamage", recievedOrderLine.iOrderLine_PhysicalDamage, SqlDbType.NVarChar);
                _dbAccessLayer.AddParameter("@Note", recievedOrderLine.iOrderLine_Note, SqlDbType.Text);
                _dbAccessLayer.AddParameter("@Organic", recievedOrderLine.iOrderLine_Organic, SqlDbType.Int);
                DataSet _orderLineDT = _dbAccessLayer.runSPDataSet(sQuery);
                if (_orderLineDT.Tables[0].Rows.Count > 0)
                {
                    _orderLine.iOrderLine_UID = Guid.Parse(_orderLineDT.Tables[0].Rows[0]["iOrderLine_UID"].ToString());

                }
                return _orderLine;
            }
        }
    }
}