using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atm11.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Currency_Country_ { get; set; }
        public string Currency_Name { get; set; }
        public decimal Conversion_Rate { get; set; }


    }
}