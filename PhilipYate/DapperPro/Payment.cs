using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDemo
{
    public class Payment
    {
        public int payment_id { get; set; }
        public int customer_id { get; set; }
        public decimal amount { get; set; }
        public DateTime payment_date { get; set; }
    }
}
