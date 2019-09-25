using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SerilogExample.Generators;

namespace MicroserviceExample.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // TODO: Get the logger using DI.
        readonly ILogger logger = Startup.LoggerFactory.ForContext<OrdersController>();

        // POST api/values
        [HttpPost]
        public void Post( [FromBody] Order order )
        {
            this.logger.Information( "Processing order {@order}", order );
        }

     
    }

}
