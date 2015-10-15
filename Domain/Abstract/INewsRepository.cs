using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract
{
    public interface INewsRepository
    {
        IEnumerable<News> News { get; }
        IEnumerable<News> GetNewsInCity(int? City);
    }
}
