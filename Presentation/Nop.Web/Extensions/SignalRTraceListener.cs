using System.Diagnostics;

namespace Nop.Web.Extensions
{
    public class SignalRTraceListener : TraceListener
    {
        public override void Write(string message) { }

        public override void WriteLine(string message)
        {
            DecisionServiceTrace.Add(new TraceMessage { Message = message });
        }
    }
}