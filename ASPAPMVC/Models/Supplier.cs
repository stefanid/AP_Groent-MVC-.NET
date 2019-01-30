using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPAPMVC.Models
{
    public class Supplier
    {
        public Guid? Supplier_UID { get; set; }

        public string Supplier_Name { get; set; }
        
        public Guid Country_UID { get; set; }

        public string Country_Name { get; set; }
    }
}