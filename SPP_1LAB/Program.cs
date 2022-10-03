using LibraryTracer;
using System;
using System.IO;
using System.Threading;
using LibraryTracer.serialization;

namespace SPP_1LAB
{
    class Program
    {
        static void Main(string[] args)
        {
            ITracer tracer = new Tracer();
            Foo foo = new Foo(tracer);
            ISerializer jsonSerializer = new JsonResult();
            ISerializer xmlSerializer = new XmlResult();

            Thread thread1 = new Thread(foo.Method1);
            thread1.Start();

            Thread thread2 = new Thread(foo.Method2);
            thread2.Start();

            foo.Method1(); 
            foo.Method2();

            thread1.Join();
            thread2.Join();

            ITraceResult result = tracer.GetTraceResult();

            string json = jsonSerializer.Serialize(result);
            string xml = xmlSerializer.Serialize(result);

            Console.WriteLine(json);
            Console.WriteLine(xml);

            Directory.CreateDirectory("out");
            File.WriteAllText("out/result.json", json);
            File.WriteAllText("out/result.xml", xml);
        }
    }

    public class Foo
    {
        private readonly Bar _bar;
        private readonly ITracer _tracer;

        public Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(tracer);
        }

        public void Method1()
        {
            _tracer.StartTrace();
            Method2();
            Thread.Sleep(50);
            _bar.InnerMethod1();
            Thread.Sleep(60);
            _tracer.StopTrace();
        }

        public void Method2()
        {
            _tracer.StartTrace();
            _bar.InnerMethod1();
            Thread.Sleep(40);
            _bar.InnerMethod2();
            Thread.Sleep(90);
            _bar.InnerMethod3();
            Thread.Sleep(30);
            _tracer.StopTrace();
        }

    }

    public class Bar
    {
        private readonly ITracer _tracer;

        public Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod1()
        {
            _tracer.StartTrace();
            Thread.Sleep(90);
            _tracer.StopTrace();
        }

        public void InnerMethod2()
        {
            _tracer.StartTrace();
            InnerMethod3();
            Thread.Sleep(70);
            _tracer.StopTrace();
        }

        public void InnerMethod3()
        {
            _tracer.StartTrace();
            InnerMethod1();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }

    }
}
