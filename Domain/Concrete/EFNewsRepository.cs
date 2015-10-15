using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Models;

namespace Domain.Concrete
{
    public class EFNewsRepository : INewsRepository
    {
        SheduleDbContext context = new SheduleDbContext();
        public IEnumerable<News> News
        {
            get
            {
                return context.News;
            }
        }

        public IEnumerable<News> GetNewsInCity(int? City)
        {
            return context.News.Where(x => x.CityId == City);
        }
    }
}
