using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Suites.Wpf.Converters
{
    internal static class ConverterHelper
    {
        public static string[] GetParameters(object parameter)
        {
            string text = parameter as string;
            if (string.IsNullOrEmpty(text))
            {
                return new string[0];
            }
            return text.Split(';');
        }

        public static bool GetBooleanParameter(string[] parameters, string name)
        {
            foreach (string a in parameters)
            {
                if (string.Equals(a, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool GetBooleanValue(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            if (value is bool?)
            {
                bool? nullable = (bool?)value;
                if (!nullable.HasValue)
                {
                    return false;
                }
                return nullable.Value;
            }
            if (value is DefaultBoolean)
            {
                return (DefaultBoolean)value == DefaultBoolean.True;
            }
            return false;
        }

        public static bool? GetNullableBooleanValue(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            if (value is bool?)
            {
                return (bool?)value;
            }
            if (value is DefaultBoolean)
            {
                DefaultBoolean defaultBoolean = (DefaultBoolean)value;
                if (defaultBoolean != DefaultBoolean.Default)
                {
                    return defaultBoolean == DefaultBoolean.True;
                }
                return null;
            }
            return null;
        }

        public static DefaultBoolean ToDefaultBoolean(bool? booleanValue)
        {
            if (booleanValue.HasValue)
            {
                if (!booleanValue.Value)
                {
                    return DefaultBoolean.False;
                }
                return DefaultBoolean.True;
            }
            return DefaultBoolean.Default;
        }

        public static bool NumericToBoolean(object value, bool inverse)
        {
            if (value == null)
            {
                return CorrectBoolean(false, inverse);
            }
            try
            {
                double num = (double)Convert.ChangeType(value, typeof(double), null);
                return CorrectBoolean(num != 0.0, inverse);
            }
            catch (Exception)
            {
            }
            return CorrectBoolean(false, inverse);
        }

        public static bool StringToBoolean(object value, bool inverse)
        {
            if (!(value is string))
            {
                return CorrectBoolean(false, inverse);
            }
            return CorrectBoolean(!string.IsNullOrEmpty((string)value), inverse);
        }

        public static Visibility BooleanToVisibility(bool booleanValue, bool hiddenInsteadOfCollapsed)
        {
            if (!booleanValue)
            {
                if (!hiddenInsteadOfCollapsed)
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        private static bool CorrectBoolean(bool value, bool inverse) => value ^ inverse;
    }

}
