using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Infrastructure
{
    public static class Stops
    {
        public static string GetNearestTime(IEnumerable<string> stops)
        {

            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("Brest Standard Time", TimeSpan.FromHours(3), "BrestTimeZone", "wintertime"));

            List<DateTime> items = new List<DateTime>();
            foreach (string stop in stops)
            {
                items.Add(DateTime.Parse(stop));
            }
            items.Add(time);
            var orderedItems = items.OrderBy(x => x.TimeOfDay);
            for (int i = 0; i < orderedItems.Count(); i++)
            {
                if (orderedItems.ElementAt(i) == time)
                {
                    if (i != orderedItems.Count() - 1)
                    {
                        return orderedItems.ElementAt(i + 1).ToString("HH:mm");
                    }
                    else
                    {
                        return orderedItems.ElementAt(0).ToString("HH:mm");
                    }
                }
            }
            return null;
        }
    }
}