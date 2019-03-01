using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks
{
    public static class TaskEx
    {
        public static readonly Task CompletedTask = Task.FromResult<object>(null);

        public static Task<T> FromException<T>(T value)
            where T : Exception
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(value);
            return tcs.Task;
        }
    }
}
