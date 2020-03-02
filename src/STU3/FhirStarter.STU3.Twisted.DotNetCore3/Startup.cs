using System;
using FhirStarter.STU3.Detonator.DotNetCore3.Filter;
using FhirStarter.STU3.Detonator.DotNetCore3.Formatters;
using FhirStarter.STU3.Instigator.DotNetCore3.Configuration;
using FhirStarter.STU3.Instigator.DotNetCore3.Helper;
using FhirStarter.STU3.Instigator.DotNetCore3.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FhirStarter.STU3.Twisted.DotNetCore3
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
            FhirDotnet3Setup(services);
        }

        // copy this method to your Startup
        public void FhirDotnet3Setup(IServiceCollection services)
        {
            var appSettings =
                StartupConfigHelper.BuildConfigurationFromJson(AppContext.BaseDirectory, "appsettings.json");
            FhirStarterConfig.SetupFhir(services, appSettings, CompatibilityVersion.Version_3_0);

            var detonator = FhirStarterConfig.GetDetonatorAssembly(appSettings["FhirStarterSettings:FhirDetonatorAssembly"]);
            var instigator = FhirStarterConfig.GetInstigatorAssembly(appSettings["FhirStarterSettings:FhirInstigatorAssembly"]);

            services.Configure<FhirStarterSettings>(appSettings.GetSection(nameof(FhirStarterSettings)));
            services.AddRouting();
            services.AddControllers(controller =>
                {
                    controller.OutputFormatters.Clear();
                    controller.InputFormatters.Clear();
                    controller.RespectBrowserAcceptHeader = true;
                    // output
                    controller.OutputFormatters.Add(new JsonFhirFormatterDotNetCore3());
                    controller.OutputFormatters.Add(new XmlFhirSerializerOutputFormatterDotNetCore3());
                    controller.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                    // input
                    controller.InputFormatters.Add(new JsonFhirInputFormatter());
                    controller.InputFormatters.Add(new XmlFhirSerializerInputFormatterDotNetCore3());

                    controller.Filters.Add(new FhirExceptionFilter());
                })
                .AddApplicationPart(instigator).AddApplicationPart(detonator).AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpContextAccessor();
            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;

                config.OutputFormatters.Add(new JsonFhirFormatterDotNetCore3());
                config.OutputFormatters.Add(new XmlFhirSerializerOutputFormatterDotNetCore3());
                config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                // input
                //config.InputFormatters.Add(new JsonFhirInputFormatter());
                //config.InputFormatters.Add(new XmlFhirSerializerInputFormatterDotNetCore3());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
