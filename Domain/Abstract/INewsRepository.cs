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
        bool Add(News news);
        bool Update(string Title, string Text, DateTime Time, int Id);
        bool Delete(int Id);
    }
}
