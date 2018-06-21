using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            #region 第一个 App.Run 委托中断了管道

            //app.Run(async context =>
            //   {
            //       await context.Response.WriteAsync("Hello, World!");//手工高亮
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello, World, Again!");
            //});
            #endregion

            #region 将多个请求委托彼此链接在一起；next 参数表示管道内下一个委托。
            //app.Use(async (context, next) =>//手工高亮
            //{
            //    await next.Invoke();//手工高亮
            //});


            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello from Use");//手工高亮
            //}); 
            #endregion

            #region Map
            //    app.MapWhen(context => {
            //        return context.Request.Path.ToString().Contains("branch");
            //    }, HandleBranch);

            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Hello from Map");
            //    });
            //}

            //private  void HandleBranch(IApplicationBuilder app)
            //{
            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Branch used.");
            //    });
            //}

            #endregion

            //app.UseMvc();

            loggerfactory.AddConsole(minLevel: LogLevel.Information);

            app.UseRequestLogger();//手工高亮

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from RequestLogger");
            });

            
        }
    }
}
