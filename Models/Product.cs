using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestDrivenAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public bool IsPhysicalProduct { get; set; }
    }
}