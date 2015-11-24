using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.SheduleParsers.Abstract;

namespace Application.Infrastructure.SheduleParserFactory.Abstract
{
    public interface ISheduleParserFactory
    {
        ISheduleParser Create(string city);
    }
}
