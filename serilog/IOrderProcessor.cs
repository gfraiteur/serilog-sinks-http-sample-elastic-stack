using SerilogExample.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SerilogExample
{
    public interface IOrderProcessor
    {
        void Process( Customer customer, Order order );
    }
}
