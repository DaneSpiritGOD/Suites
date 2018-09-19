using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class DateTimeExtensions
    {
        public static double ToJulianDate(this DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }
    }
}
