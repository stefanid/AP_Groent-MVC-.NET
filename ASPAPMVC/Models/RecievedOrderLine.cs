using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Models
{
    public class RecievedOrderLine
    {
        public Guid? iOrderLine_UID { get; set; }

        public string iOrderLine_DeliveryLotNr { get; set; }

        public string ProductVariation_Name { get; set; }

        public int iOrderLine_Pallets { get; set; }

        public Guid iOrder_UID { get; set; }

        public int LineNumber { get; set; }

        public Guid ProductVariation_UID { get; set; }

        public Guid BoxType_UID { get; set; }

        public string BoxType_Name { get; set; }

        public decimal iOrderLine_OrderNetto { get; set; }

        public decimal iOrderLine_OrderBrutto { get; set; }

        public decimal iOrderLine_RecivedNetto { get; set; }

        public decimal iOrderLine_RecivedBrutto { get; set; }

        public int iOrderLine_Colli { get; set; }

        public decimal iOrderLine_Temperature { get; set; }

        public string Extra { get; set; }

        public string iOrderLine_ErrSize { get; set; }

        public string iOrderLine_ErrForm { get; set; }

        public string iOrderLine_ErrColor { get; set; }

        public string iOrderLine_InsectDamage { get; set; }

        public string iOrderLine_FungiDamage { get; set; }

        public string iOrderLine_PhysicalDamage { get; set; }

        public string iOrderLine_Note { get; set; }

        public int iOrderLine_Organic { get; set; }

        public bool Organic { get; set; }
    }
}