using System;
using System.Threading;
using Bogus;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Elasticsearch;
using SerilogExample.Generators;

namespace SerilogExample
{
    public class Program
    {
        static void Main()
        {
            SelfLog.Enable( Console.Error );

            ILogger logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty( "Application", "ClientExample" )
                .WriteTo.Console()
                .WriteTo.Elasticsearch( new ElasticsearchSinkOptions( new Uri( "http://localhost:9200" ) )
                {
                    BatchPostingLimit = 1, // For demo.
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.WriteToSelfLog,
                    FailureCallback = e => Console.WriteLine( "Unable to submit event " + e.MessageTemplate ),
                    
                } )
                .CreateLogger();

            IOrderProcessor orderProcessor = new WebClientOrderProcessor( logger );

            var customerGenerator = new CustomerGenerator();
            var orderGenerator = new OrderGenerator();

            for ( int i = 0 ; i < 10 ; i++ )
            {
                var customer = customerGenerator.Generate();
                var order = orderGenerator.Generate();
                order.CustomerId = customer.Id;

                orderProcessor.Process( customer, order );
                Thread.Sleep( 200 );

            }


            Log.CloseAndFlush();
            
        }
    }
}
