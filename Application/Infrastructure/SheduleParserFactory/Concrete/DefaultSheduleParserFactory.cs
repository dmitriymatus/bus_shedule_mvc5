using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Infrastructure.SheduleParserFactory.Abstract;
using Domain.SheduleParsers.Abstract;
using Domain.SheduleParsers.Concrete;

namespace Application.Infrastructure.SheduleParserFactory.Concrete
{
    public class DefaultSheduleParserFactory : ISheduleParserFactory
    {
        public ISheduleParser Create(string city)
        {
            var parser = SheduleParsers.Parsers.Where(x => x.Key == city);
            if(parser.Any())
            {
                return parser.First().Value.Invoke();
            }
            return new BrestSheduleParser();
        }
    }
}
