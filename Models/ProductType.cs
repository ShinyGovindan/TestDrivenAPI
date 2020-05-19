using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestDrivenAPI.Models
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public int ProductId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}