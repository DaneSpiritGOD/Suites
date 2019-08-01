using System.Runtime.CompilerServices;

namespace System
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string msg) : base(msg) { }
    }

    public class StringNullOrEmptyException : RuntimeException
    {
        private StringNullOrEmptyException(string variableName, string method)
            : base($"Variable(`{variableName}`) in method(`{method}`) must be not be null or empty.") { }

        public static string Assert(string arg, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new StringNullOrEmptyException(exceptionShowName, methodName);
            }

            return arg;
        }
    }

    public class StringNullOrWhiteSpaceException : RuntimeException
    {
        private StringNullOrWhiteSpaceException(string variableName, string method)
            : base($"Variable(`{variableName}`) in method(`{method}`) must be not be null or whitespace.") { }

        public static string Assert(string arg, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new StringNullOrWhiteSpaceException(exceptionShowName, methodName);
            }

            return arg;
        }
    }

    public class NamedNullException : RuntimeException
    {
        private NamedNullException(string variableName, string method)
            : base($"Variable(`{variableName}`) in method(`{method}`) is null.") { }

        public static T Assert<T>(T arg, string exceptionShowName, [CallerMemberName]string methodName = null)
            where T : class
            => arg ?? throw new NamedNullException(exceptionShowName, methodName);
    }

    public class NumberOutOfRangeException<T> : RuntimeException where T : struct, IComparable
    {
        private NumberOutOfRangeException(T real, T low, T high, string variableName, string method)
            : base($"Variable(`{variableName}`) in method(`{method}`) : {real} is out of range ({low},{high}).") { }

        public static T Assert(T arg, T low, T high, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (arg.CompareTo(high) > 0 || arg.CompareTo(low) < 0)
            {
                throw new NumberOutOfRangeException<T>(arg, low, high,
                    exceptionShowName, methodName);
            }

            return arg;
        }
    }

    public class NotTrueException : RuntimeException
    {
        private NotTrueException(string predictName, string method)
            : base($"Predict(`{predictName}`) in method(`{method}`) is not true.") { }

        public static void Assert(Func<bool> predict, string predictName, [CallerMemberName]string methodName = null)
        {
            if (!predict())
            {
                throw new NotTrueException(predictName, methodName);
            }
        }

        public static void Assert(bool predictResult, string predictName, [CallerMemberName]string methodName = null)
        {
            if (!predictResult)
            {
                throw new NotTrueException(predictName, methodName);
            }
        }
    }
}
