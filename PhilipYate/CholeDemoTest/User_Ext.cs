using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CholeDemoTest
{
    [Table("user_ext")]
    public class User_Ext
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
