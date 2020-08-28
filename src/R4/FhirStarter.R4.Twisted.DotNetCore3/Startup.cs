using System;
using System.Diagnostics;
using FhirStarter.R4.Detonator.DotNetCore3.Filter;
using FhirStarter.R4.Detonator.DotNetCore3.Formatters;
using FhirStarter.R4.Instigator.DotNetCore3.Configuration;
using FhirStarter.R4.Instigator.DotNetCore3.Diagnostics;
using FhirStarter.R4.Instigator.DotNetCore3.Helper;
using FhirStarter.R4.Instigator.DotNetCore3.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FhirStarter.R4.Twisted.DotNetCore3
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

                    controller.Filters.Add(typeof(FhirExceptionFilter));
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

                
            });
            services.AddSingleton<DiagnosticObserver>();
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            DiagnosticListener diagnosticListenerSource, DiagnosticObserver diagnosticObserver)
        {
            diagnosticListenerSource.Subscribe(diagnosticObserver);
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
