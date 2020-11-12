using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreMentoring.App.Extensions;
using NetCoreMentoring.App.Infrastructure;
using NetCoreMentoring.Core.Extensions;
using NetCoreMentoring.Data.Extensions;

namespace NetCoreMentoring.App
{
    public class Startup
    {
        private const string DbConnectionStringName = "MyConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApp(Configuration);
            services.AddCore(Configuration);
            services.AddData(Configuration, DbConnectionStringName);

            services.AddAutoMapper(
                typeof(App.Mapping.MappingProfile).Assembly,
                typeof(Core.Mapping.MappingProfile).Assembly);

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ActionInvocationLoggingFilter));
            });
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMiddleware<ImageCacheMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "blog",
                    pattern: "images/{categoryId}",
                    defaults: new { controller = "Category", action = "Picture" });
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}