using System;
using System.Diagnostics;
using OneAmEngine;

namespace OpenC1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            SetupTracing();

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }

        /// <summary>
        /// The parsers use Trace.Assert as a marker for "this data looks unexpected".
        /// The project used to be built without TRACE, so those calls vanished entirely.
        /// They are active now - without this redirection every hit would pop up a modal
        /// dialog mid-game. Instead it goes to OpenC1.log.
        /// </summary>
        static void SetupTracing()
        {
            foreach (TraceListener listener in Trace.Listeners)
            {
                if (listener is DefaultTraceListener def)
                    def.AssertUiEnabled = false;
            }
            Trace.Listeners.Add(new LoggerTraceListener());
        }

        class LoggerTraceListener : TraceListener
        {
            public override void Write(string message) => Logger.Log(message);
            public override void WriteLine(string message) => Logger.Log(message);

            public override void Fail(string message, string detailMessage)
            {
                Logger.Log("ASSERT: " + message + " " + detailMessage);
                Logger.Log(new StackTrace(true).ToString());
            }
        }
    }
}
