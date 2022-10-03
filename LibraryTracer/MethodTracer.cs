using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LibraryTracer
{
    public class MethodTracer : ITracer
    {
        private readonly string name;
        private readonly string ClassName;

        private readonly Stopwatch _stopwatch = new();
        private readonly List<MethodTracer> MethodTracers = new();

        public MethodTracer()
        {
            MethodBase method = new StackFrame(3, false).GetMethod();
            name = method?.Name;
            ClassName = method?.DeclaringType?.FullName;
        }

        public void AddChildMethod(MethodTracer methodTracer)
        {
            MethodTracers.Add(methodTracer);
        }

        public void StartTrace()
        {
            _stopwatch.Start();
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
        }

        public ITraceResult GetTraceResult()
        {
            var results = MethodTracers
                .Select(threadTracer => (MethodTraceResult)threadTracer.GetTraceResult())
                .ToList();

            return new MethodTraceResult(name, ClassName, _stopwatch.ElapsedMilliseconds, results);
        }

    }
}
