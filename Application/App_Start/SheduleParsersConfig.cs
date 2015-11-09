using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.SheduleParsers.Abstract;
using Domain.SheduleParsers.Concrete;

namespace Application
{
    public class SheduleParsersConfig
    {
        public static void RegisterParsers(Dictionary<string, Func<ISheduleParser>> Parsers)
        {
            Parsers.Add("брест",() => new BrestSheduleParser());
        }
    }
}
