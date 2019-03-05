namespace System.Threading.Tasks
{
    public static class TaskEx
    {
#if NETSTANDARD2_0
        public static readonly Task CompletedTask = Task.CompletedTask;

        public static Task<T> FromException<T>(T value)
            where T : Exception
            => Task.FromException<T>(value);
#elif NET45
        public static readonly Task CompletedTask = Task.FromResult(false);

        public static Task<T> FromException<T>(T value)
            where T : Exception
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(value);
            return tcs.Task;
        }
#endif
    }
}
