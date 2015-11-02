using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Infrastructure
{
    public static class Stops
    {
        public static string GetNearestTime(string value)
        {
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}");
            MatchCollection matches = reg.Matches(value);

            var stops = matches.Cast<Match>().Select(x => x.Value);
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("Brest Standard Time", TimeSpan.FromHours(3), "BrestTimeZone", "wintertime"));
            var orderedItems = stops.Select(x => DateTime.Parse(x)).OrderBy(x => x);
            var items = orderedItems.SkipWhile(x => x <= time);         
            return items.Any() ? items.FirstOrDefault().ToString("HH:mm") : orderedItems.FirstOrDefault().ToString("HH:mm");
        }
    }
}