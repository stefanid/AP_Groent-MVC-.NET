using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Models
{
    public class Order
    {
        public Guid? iOrder_UID { get; set; }

        public string Supplier_Name { get; set; }

        public int iOrder_ID { get; set; }

        public Guid Supplier_UID { get; set; }

        public string Order_Status { get; set; }

        public string iOrder_Note1 { get; set; }

        public string iOrder_Note2 { get; set; }

        public string iOrder_SupplierRef { get; set; }

        public int iOrder_EconiOrderID { get; set; }

        public DateTime Order_Date { get; set; }

        public int iOrder_iOrderDate { get; set; }

        public int iOrder_DeliveryDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int iOrder_ProductionDate { get; set; }

        public int iOrder_TypeID { get; set; }

        public int iOrder_StatusID { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Guid iOrder_CreatorUID { get; set; }

        public DateTime iOrder_CreateDate { get; set; }

        public Guid iOrder_LastUpdaterUID { get; set; }

        public DateTime iOrder_LastUpdate { get; set; }

        public int iOrder_Organic { get; set; }

        public bool Organic { get; set; }

        public string StringifyOrganic { get; set; }

        public int iOrder_ConfirmationShipped { get; set; }

        public string StringOrderDate { get; set; }

        public string StringDeliveryDate { get; set; }

        public string StringCreateDate { get; set; }

        public ShippingStatus ShipStatus { get; set; }
    }
}