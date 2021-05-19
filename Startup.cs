using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accesosIp.Data;
using accesosIp.DBContexts;
using accesosIp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Session;
using accesosIp.Hubs;
using accesosIp.Services;

namespace accesosIp
{
    public class Startup
    {
        private readonly IServiceProvider serviceProvider;
        public Startup(IConfiguration configuration,IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Repository>();

            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"),
               sqlServerOptionsAction: sqlOtions => {
                   sqlOtions.EnableRetryOnFailure();
               }));

            services.AddHttpContextAccessor();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "Web-Cliente";
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
            });
            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddScoped<IDatabaseChangeNotificationService, SqlDependencyService>();
            services.AddSignalR();
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IDatabaseChangeNotificationService databaseChangeNotificationService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseHttpContextItemsMiddleware();
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseSignalR(x =>
            {
                x.MapHub<ChatHub>("/SessionHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });

            databaseChangeNotificationService.Config(-1);
        }
    }
}
