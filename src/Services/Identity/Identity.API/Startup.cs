using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.Identity.API.Configuration;
using drDotnet.Services.Identity.API.Configuration.Constants;
using drDotnet.Services.Identity.API.Configuration.Root;
using drDotnet.Services.Identity.API.Data;
using drDotnet.Services.Identity.API.Helpers;
using drDotnet.Services.Identity.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace drDotnet.Services.Identity.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var rootConfiguration = CreateRootConfiguration();
            services.AddSingleton(rootConfiguration);

            services.AddControllersWithViews();

            RegisterDbContexts(services);

            services.AddIdentity<AppIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Resources)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<AppIdentityUser>();

            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContexts(Configuration);
        }

        protected IRootConfiguration CreateRootConfiguration()
        {
            var conf = new RootConfiguration();
            Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(conf.RegisterConfiguration);
            return conf;
        }
    }
}
