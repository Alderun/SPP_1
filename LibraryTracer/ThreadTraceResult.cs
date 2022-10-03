using System.Collections.Generic;
using System.Xml.Serialization;

namespace LibraryTracer
{
    public class ThreadTraceResult : ITraceResult
    {
        [XmlAttribute("id")]
        public long Id { get; init; }
        [XmlAttribute("time")]
        public long Time { get; init; }
        [XmlElement("method")]
        public List<MethodTraceResult> Methods { get; init; }

        public ThreadTraceResult() { }

        public ThreadTraceResult(long id, long time, List<MethodTraceResult> methods)
        {
            Id = id;
            Time = time;
            Methods = methods;
        }

    }
}
