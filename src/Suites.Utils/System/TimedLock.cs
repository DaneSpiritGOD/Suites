using System.Threading;

// Thanks to Eric Gunnerson for recommending this be a struct rather
// than a class - avoids a heap allocation.
// (In Debug mode, we make it a class so that we can add a finalizer
// in order to detect when the object is not freed.)
// Thanks to Chance Gillespie and Jocelyn Coulmance for pointing out
// the bugs that then crept in when I changed it to use struct...

namespace System
{
#if DEBUG
    public class TimedLock : IDisposable
#else
    public struct TimedLock : IDisposable
#endif
    {
        public static TimedLock Lock(object o)
            => Lock(o, TimeSpan.FromSeconds(10));

        public static TimedLock Lock(object o, TimeSpan timeout)
        {
            var tl = new TimedLock(o);
            if (!Monitor.TryEnter(o, timeout))
            {
#if DEBUG
                System.GC.SuppressFinalize(tl);
#endif
                throw new LockTimeoutException();
            }

            return tl;
        }

        private TimedLock(object o)
        {
            target = o;
        }
        private readonly object target;

        public void Dispose()
        {
            Monitor.Exit(target);

            // It's a bad error if someone forgets to call Dispose,
            // so in Debug builds, we put a finalizer in to detect
            // the error. If Dispose is called, we suppress the
            // finalizer.
#if DEBUG
            GC.SuppressFinalize(this);
#endif
        }

#if DEBUG
        ~TimedLock()
        {
            // If this finalizer runs, someone somewhere failed to
            // call Dispose, which means we've failed to leave
            // a monitor!
            System.Diagnostics.Debug.Fail("Undisposed lock");
        }
#endif

    }
    public class LockTimeoutException : System.Exception
    {
        public LockTimeoutException() : base("Timeout waiting for lock") { }
    }
}
