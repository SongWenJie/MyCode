using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDemo
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public int Age { get; set; }

        public int Status { get; set; }

        public string Remark { get; set; }

        public User_Ext user_ext { get; set; }
    }
}
