using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestDrivenAPI.Models
{
    public class ProductBusinessRule
    {
        public int RuleId { get; set; }
        public int ProductTypeId { get; set; }
        public string BusinessRuleName { get; set; }
        public bool isActive { get; set; }
        
    }
}