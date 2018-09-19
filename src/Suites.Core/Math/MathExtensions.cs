using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class MathExtensions
    {
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="strtemp"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string strtemp)
        {
            try
            {
                Convert.ToDouble(strtemp);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DoubleValueEquals(this double a, double b, int precision = 3)
        {
            double num = Math.Round(a, precision);
            double num2 = Math.Round(b, precision);
            return num == num2;
        }
    }
}
