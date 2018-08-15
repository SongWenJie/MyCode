using System;
using System.Collections.Generic;
using System.Text;

namespace PhilipYate.Application
{
    public class ReturnData
    {
        public ReturnData(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }


        /// <summary>
        /// 状态码
        /// </summary>
        public int code;
        /// <summary>
        /// 操作消息
        /// </summary>
        public string msg;
    }
}
