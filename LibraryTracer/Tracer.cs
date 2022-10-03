using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LibraryTracer
{
    public class Tracer : ITracer
    {
        private readonly List<ITracer> ThreadTracers = new();
        private readonly ReaderWriterLockSlim locker = new();

        public void StartTrace()
        {
            int id = Thread.CurrentThread.ManagedThreadId;

            var CurrentThreadTracer = GetThreadTracer(id);

            if (CurrentThreadTracer == null)
            {
                CurrentThreadTracer = new ThreadTracer(id);
                AddThreadTracer(CurrentThreadTracer);
            }

            CurrentThreadTracer.StartTrace();
        }

        public void StopTrace()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            GetThreadTracer(id)?.StopTrace();
        }

        public ITraceResult GetTraceResult()
        {

            locker.EnterReadLock();
            try
            {
                var results = ThreadTracers
                    .Select(threadTracer => (ThreadTraceResult)threadTracer.GetTraceResult())
                    .ToList();

                return new TraceResult(results);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }
        private ITracer GetThreadTracer(int id)
        {
            locker.EnterReadLock();
            try
            {
                return ThreadTracers
                .Cast<ThreadTracer>()
                .FirstOrDefault(threadTracer => threadTracer.Id == id);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        private void AddThreadTracer(ITracer threadTracer)
        {
            locker.EnterWriteLock();
            try
            {
                ThreadTracers.Add(threadTracer);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }


    }
}
