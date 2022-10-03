using System.Collections.Generic;
using System.Diagnostics;

namespace LibraryTracer
{
    public class ThreadTracer : ITracer
    {
        public long Id { get; init; }
        private MethodTracer CurrentMethodTracer;

        private readonly Stopwatch stopwatch = new();
        private readonly Stack<MethodTracer> MethodStack = new();
        private readonly List<MethodTracer> MethodTracers = new();

        public ThreadTracer(long id)
        {
            Id = id;
        }

        public void StartTrace()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            MethodTracer newMethodTracer = new MethodTracer();

            if (CurrentMethodTracer != null)
            {
                MethodStack.Push(CurrentMethodTracer);
                CurrentMethodTracer.AddChildMethod(newMethodTracer);
            }
            else
            {
                MethodTracers.Add(newMethodTracer);
            }

            CurrentMethodTracer = newMethodTracer;
            CurrentMethodTracer.StartTrace();
        }

        public void StopTrace()
        {
            if (CurrentMethodTracer != null)
            {
                CurrentMethodTracer.StopTrace();
                CurrentMethodTracer = MethodStack.Count != 0 ? MethodStack.Pop() : null;
            }
        }

        public ITraceResult GetTraceResult()
        {
            long executionTime = 0;

            List<MethodTraceResult> results = new List<MethodTraceResult>();
            foreach (var methodTracer in MethodTracers)
            {
                var result = (MethodTraceResult)methodTracer.GetTraceResult();
                executionTime += result.Time;
                results.Add(result);
            }

            return new ThreadTraceResult(Id, executionTime, results);
        }

    }
}

