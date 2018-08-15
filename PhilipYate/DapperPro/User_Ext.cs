using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDemo
{
    
    public class User_Ext
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
