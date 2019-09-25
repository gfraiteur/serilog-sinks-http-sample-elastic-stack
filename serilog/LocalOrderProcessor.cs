using Newtonsoft.Json;
using Serilog;
using SerilogExample.Generators;
using System.Net.Http.Formatting;

namespace SerilogExample
{
    public class LocalOrderProcessor : IOrderProcessor
    {
        private readonly ILogger logger;

        public LocalOrderProcessor( ILogger logger )
        {
            this.logger = logger.ForContext<LocalOrderProcessor>();
        }

        public void Process( Customer customer, Order order )
        {
            this.logger.Information( "{@customer} placed {@order}", customer, order );
        }
    }
}
