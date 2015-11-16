using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Domain.Models
{
    [Flags]
    public enum Days : short
    {
        [Description("Пн")]
        Monday = 1,
        [Description("Вт")]
        Tuesday = 2,
        [Description("Ср")]
        Wednesday = 4,
        [Description("Чт")]
        Thursday = 8,
        [Description("Пт")]
        Friday = 16,
        [Description("Сб")]
        Saturday = 32,
        [Description("Вс")]
        Sunday = 64,
        [Description("Рабочие")]
        Weekend = Saturday | Sunday,
        [Description("Выходные")]
        Working = Monday | Tuesday | Wednesday | Thursday | Friday,
        [Description("Ежедневно")]
        Daily = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
    }

    public static class DaysHelpers
    {
        public static string ToDescription(this Days days)
        {
            FieldInfo fi = days.GetType().GetField(days.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return days.ToString();
        }
    }
}
