using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string msg) : base(msg) { }
    }

    public class StringNullOrEmptyException : RuntimeException
    {
        public StringNullOrEmptyException(string variableName) : base($"{variableName} - must be not be null or empty.") { }

        public static string Assert(string arg, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrEmpty(arg)) throw new StringNullOrEmptyException($"variable: {exceptionShowName} in method: {methodName}");
            return arg;
        }
    }

    public class StringNullOrWhiteSpaceException : RuntimeException
    {
        public StringNullOrWhiteSpaceException(string variableName) : base($"{variableName} - must be not be null or empty or whitespace.") { }

        public static string Assert(string arg, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(arg)) throw new StringNullOrWhiteSpaceException($"variable: {exceptionShowName} in method: {methodName}");
            return arg;
        }
    }

    public class NamedNullException : RuntimeException
    {
        public NamedNullException(string variableName) : base($"{variableName} is null.") { }

        public static T Assert<T>(T arg, string exceptionShowName, [CallerMemberName]string methodName = null) where T : class
        {
            return arg ?? throw new NamedNullException($"variable: {exceptionShowName} in method: {methodName}");
        }
    }

    public class NumberOutOfRangeException<T> : RuntimeException where T : struct, IComparable
    {
        public NumberOutOfRangeException(T real, T low, T high, string exceptionShowName)
            : base($"{exceptionShowName}: {real} is out of range ({low},{high}).") { }

        public static T Assert(T arg, T low, T high, string exceptionShowName, [CallerMemberName]string methodName = null)
        {
            if (arg.CompareTo(high) > 0 || arg.CompareTo(low) < 0) throw new NumberOutOfRangeException<T>(
                arg,
                low,
                high,
                $"variable: {exceptionShowName} in method: {methodName}");
            return arg;
        }
    }

    public class NotTrueException : RuntimeException
    {
        public NotTrueException(string predictName) : base($"Prediction: {predictName} is not true.") { }

        public static void Assert(Func<bool> predict, string predictName, [CallerMemberName]string methodName = null)
        {
            if (!predict()) throw new NotTrueException($"predict: {predictName} in method: {methodName}");
        }

        public static void Assert(bool predictResult, string predictName, [CallerMemberName]string methodName = null)
        {
            if (!predictResult) throw new NotTrueException($"predict: {predictName} in method: {methodName}");
        }

        public static T Assert<T>(T value, bool condition, string valueName, [CallerMemberName]string methodName = null)
        {
            if (!condition) throw new NotTrueException($"{valueName} does not meet the condition in method: {methodName}.");
            return value;
        }
    }
}
