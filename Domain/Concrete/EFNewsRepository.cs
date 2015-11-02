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

        public bool Add(News news)
        {
            City city = context.Cities.Find(news.City.Id);
            news.City = city;
            try
            {
                context.News.Add(news);
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
