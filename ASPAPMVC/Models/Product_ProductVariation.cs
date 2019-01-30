using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Models
{
    public class Product_ProductVariation
    {
        public string Product_Name { get; set; }

        public string ProductVariation_Name { get; set; }

        public Guid ProductVariation_UID { get; set; }
    }
}