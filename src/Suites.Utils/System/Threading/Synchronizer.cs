namespace System.Threading
{
    /// <summary>
    /// Allows you to encapsulate an instance of a type that needs to be shared
    /// between multiple threads, such that access to the instance is easily
    /// synchronised.
    /// </summary>
    public class Synchronizer<TImpl, TIRead, TIWrite>
        where TImpl : TIWrite, TIRead
    {
        // Our actual locker, that will synchronise access to our _shared instance.
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        // Our actual shared resource.
        private TImpl _shared;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:threadsynchronization.Synchronizer`1"/> class.
        /// </summary>
        /// <param name="shared">Instance that needs synchronised access in multiple threads.</param>
        public Synchronizer(TImpl shared)
        {
            _shared = shared;
        }

        /// <summary>
        /// Enters a read lock giving the caller access to the shared instance in
        /// "read only" mode.
        /// </summary>
        /// <param name="functor">Functor.</param>
        public void Read(Action<TIRead> functor)
        {
            _lock.EnterReadLock();
            try
            {
                functor(_shared);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Enters a write lock giving the caller access to the shared resource
        /// in "read and write" mode.
        /// </summary>
        /// <param name="functor">Functor.</param>
        public void Write(Action<TIWrite> functor)
        {
            _lock.EnterWriteLock();
            try
            {
                functor(_shared);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Enters a write lock giving the caller access to the shared resource
        /// in "read and write" mode, for then to reassign the shared object to
        /// the value returned from the Func.
        /// 
        /// Useful for changing immutable instances.
        /// </summary>
        /// <param name="functor">Functor.</param>
        public void Assign(Func<TIWrite, TImpl> functor)
        {
            _lock.EnterWriteLock();
            try
            {
                _shared = functor(_shared);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

    /// <summary>
    /// Simplified syntax where you cannot modify the shared type and implement
    /// your own read and write interfaces.
    /// 
    /// Notice, when using this class you are on your own in regards to making sure
    /// you never actually modify the shared instance inside a "read only" delegate.
    /// </summary>
    public class Synchronizer<TImpl> : Synchronizer<TImpl, TImpl, TImpl>
    {
        public Synchronizer(TImpl shared)
            : base(shared)
        { }
    }
}
