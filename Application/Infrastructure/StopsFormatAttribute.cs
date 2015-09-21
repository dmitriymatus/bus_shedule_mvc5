using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;
using System.Text;

namespace Application.Infrastructure
{
    public class StopsFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var stops = value as string;
            if (stops == null)
            {
                return false;
            }

            Regex reg = new Regex(@"\d{1,2}:\d{1,2}");
            MatchCollection matches = reg.Matches(stops);
            if (matches.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}