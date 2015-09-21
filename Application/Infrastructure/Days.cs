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

            return result;

        }
    }
}