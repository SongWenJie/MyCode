using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WatchDemo.Controllers
{
    /// <summary>
    /// Nuget安装Watcher修改代码 监控 自动重启服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MathController : Controller
    {
        public int Sum(int a,int b)
        {
            return a + b;
        }

        public int Product(int a,int b)
        {
            return a * b;
        }
    }
}
