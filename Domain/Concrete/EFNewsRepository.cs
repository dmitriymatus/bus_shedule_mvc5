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

        public bool Add(string Title, string Text, DateTime Time, int? CityId)
        {
            try
            {
                Models.News item = new Models.News()
                {
                    Title = Title,
                    Text = Text,
                    Time = Time,
                    CityId = CityId
                };
                context.News.Add(item);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int Id)
        {
            var items = context.News.Where(x => x.Id == Id);
            if (items.Any())
            {
                var item = items.First();
                context.News.Remove(item);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<News> GetNewsInCity(int? City)
        {
            return context.News.Where(x => x.CityId == City);
        }

        public bool Update(string Title, string Text, DateTime Time, int Id)
        {
            var items = context.News.Where(x => x.Id == Id);
            if (items.Any())
            {
                var item = items.First();
                item.Title = Title;
                item.Text = Text;
                item.Time = Time;
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
