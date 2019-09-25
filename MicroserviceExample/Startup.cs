using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace MicroserviceExample
{
    public class Startup
    {
        public static Serilog.ILogger LoggerFactory;

        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;

            LoggerFactory = new LoggerConfiguration()
              .Enrich.WithProperty( "Application", "MicroserviceExample" )
              .Enrich.FromLogContext()
              .MinimumLevel.Debug()
                       .WriteTo.Elasticsearch( new ElasticsearchSinkOptions( new Uri( "http://localhost:9200" ) )
                       {
                           BatchPostingLimit = 1, // For demo.
                           AutoRegisterTemplate = true,
                           AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                           EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.WriteToSelfLog,
                           FailureCallback = e => Console.WriteLine( "Unable to submit event " + e.MessageTemplate ),
                       } )
              .WriteTo.Console(
                  outputTemplate:
                  "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Indent:l}{Message:l}{NewLine}{Exception}" )
              .CreateLogger();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc( delegate ( MvcOptions options )
            {
                options.Filters.Add( typeof( LoggingActionFilter ) );
            } ).SetCompatibilityVersion( CompatibilityVersion.Version_2_2 );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            app.UseMvc();
        }
    }
}
