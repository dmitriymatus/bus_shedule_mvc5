using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Infrastructure
{
    public class Days
    {
        public static string GetDays(IEnumerable<string> days)
        {
            string result = days.FirstOrDefault();
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                if (days.Contains("Выходные")) result = "Выходные";
            }
            else if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                if (days.Contains("Рабочие")) result = "Рабочие";
            }

            if (!days.Contains("Рабочие") && !days.Contains("Рабочие") && !days.Contains("Ежедневно"))
            {
                var day = DayOfWeekConverter();
                foreach(var item in days)
                {
                    if (item.Contains(day)) result = item; 
                }

            }

            return result;

        }

        private static string DayOfWeekConverter()
        {
            var day = DateTime.Today.DayOfWeek;
            switch(day)
            { 
                case DayOfWeek.Monday: { return "ПН"; }
                case DayOfWeek.Tuesday: { return "ВТ"; }
                case DayOfWeek.Wednesday: { return "СР"; }
                case DayOfWeek.Thursday: { return "ЧТ"; }
                case DayOfWeek.Friday: { return "ПТ"; }
                case DayOfWeek.Sunday: { return "ВС"; }
            }
            return null;
        }


    }
}