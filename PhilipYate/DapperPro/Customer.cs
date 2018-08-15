using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDemo
{
    public class Customer
    {
        public int customer_id { get; set; }
        public string  first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }

        public List<Payment> Payments { get; set; }
    }
}
