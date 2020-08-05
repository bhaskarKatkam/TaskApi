using System;
using System.Collections.Generic;

namespace DataParserApp.Models
{
    public partial class Customer
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public int CustType { get; set; }
        public decimal TotAmt { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
