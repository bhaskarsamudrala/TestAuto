using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Company.Platform.automation.App.web
{
    public class ConsoleTraceListener : TextWriterTraceListener
    {
        public ConsoleTraceListener() : base(new ConsoleTextWriter())
        {
        }

        public override void Close()
        {
        }
    }

    public class ConsoleTextWriter : TextWriter
    {
        public override Encoding Encoding => Console.Out.Encoding;

        public override void Write(string value)
        {
            Console.Out.Write(value);
        }

        public override void WriteLine(string value)
        {
            Console.Out.WriteLine(value);
        }
    }
}