using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace StaticFileDemo
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
            services.AddDirectoryBrowser();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var provider = new FileExtensionContentTypeProvider();                          
            //provider.Mappings[".png"] = "image/png";     
            
            provider.Mappings.Remove(".png");

            //app.UseDefaultFiles();

            //app.UseDefaultFiles(new DefaultFilesOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = new PathString("/StaticFiles")
            //});

            //为了能够启用静态文件服务，你必须配置中间件（middleware），把静态文件中间件加入到管道内。
            //静态文件中间件能够通过下述方法来配置：在你的项目中增加 Microsoft.AspNetCore.StaticFiles 包依赖，
            // 然后从 Startup.Configure 调用 UseStaticFiles 扩展方法：
            //app.UseStaticFiles();

            //使用 UseStaticFiles 将其他目录下的内容也向外提供服务
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = new PathString("/StaticFiles"),
                ContentTypeProvider = provider
                //DefaultContentType = "image/png",
                //ServeUnknownFileTypes = false
            }); 

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
            //    RequestPath = new PathString("/images")
            //});

            //允许对外展示文件目录，若设置的请求路径一样，则默认使用第一个
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });


            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
            //    RequestPath = new PathString("/images")
            //});

            //UseFileServer 包含了 UseStaticFiles 、UseDefaultFiles 和 UseDirectoryBrowser 的功能
            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = new PathString("/StaticFiles"),
            //    EnableDirectoryBrowsing = false,
            //    EnableDefaultFiles = false
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
