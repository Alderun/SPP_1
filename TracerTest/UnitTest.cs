using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryTracer;
using System.Threading;
using System.Collections.Generic;

namespace TracerTest
{
    [TestClass]
    public class UnitTesting
    {
        Tracer tracer = new Tracer();

        void MethodTrace()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }


        [TestMethod]
        public void TestMethodName()
        {
            const string expected = "MethodTrace";
            MethodTrace();
            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(1, result.Threads[0].Methods.Count);
            Assert.AreEqual(expected, result.Threads[0].Methods[0].Name);
        }

        [TestMethod]
        public void TestThreadId()
        {
            long expected = Thread.CurrentThread.ManagedThreadId;
            MethodTrace();
            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(1, result.Threads.Count);
            Assert.AreEqual(expected, result.Threads[0].Id);
        }

        [TestMethod]
        public void TestClassName()
        {
            string expected = typeof(UnitTesting).FullName;
            MethodTrace();
            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(expected, result.Threads[0].Methods[0].ClassName);
        }

        [TestMethod]
        public void TestThreadsCount()
        {
            List<Thread> threads = new List<Thread>();
            var expectedThreadsCount = 4;
            for (int i = 0; i < expectedThreadsCount; i++)
            {
                threads.Add(new Thread(MethodTrace));
                threads[i].Start();
                threads[i].Join();
            }

            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(threads.Count, result.Threads.Count);
        }

    }
}
