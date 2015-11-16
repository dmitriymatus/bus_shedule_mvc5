using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Infrastructure
{
    public static class Stops
    {
        public static string GetNearestTime(IEnumerable<TimeSpan> values)
        {
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("Brest Standard Time", TimeSpan.FromHours(3), "BrestTimeZone", "wintertime")).TimeOfDay;
            var orderedItems = values.OrderBy(x => x);
            var items = orderedItems.SkipWhile(x => x <= time);         
            return items.Any() ? items.FirstOrDefault().ToString("hh\\:mm") : orderedItems.FirstOrDefault().ToString("hh\\:mm");
        }
    }
}