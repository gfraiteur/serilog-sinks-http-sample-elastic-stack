using Serilog;
using Serilog.Context;
using SerilogExample.Generators;
using System;
using System.Net.Http;

namespace SerilogExample
{
    public class WebClientOrderProcessor : IOrderProcessor
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient = new HttpClient();
        
        public WebClientOrderProcessor( ILogger logger )
        {
            this.logger = logger.ForContext<WebClientOrderProcessor>();
        }

        public void Process( Customer customer, Order order )
        {

            Guid operationId = Guid.NewGuid();

            using (LogContext.PushProperty( "OperationId", operationId ))
            {

                this.logger.Information( "Invoking microservice to process order {@order}.", order );

                this.httpClient.DefaultRequestHeaders.Remove( "Request-Id" );
                this.httpClient.DefaultRequestHeaders.Add( "Request-Id", operationId.ToString() );
                this.httpClient.PostAsJsonAsync( "https://localhost:44332/api/orders", order ).Wait();
            }
        }
    }
}
