using System.Collections.Generic;
using System.Xml.Serialization;

namespace LibraryTracer
{
    public class TraceResult:ITraceResult
    {
        [XmlElement("thread")]
        public List<ThreadTraceResult> Threads { get; init; }

        public TraceResult() { }

        public TraceResult(List<ThreadTraceResult> threads)
        {
            Threads = threads;
        }

    }
}
