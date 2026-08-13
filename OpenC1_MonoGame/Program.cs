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
        /// Die Parser benutzen Trace.Assert als Marker fuer "diese Daten sehen unerwartet
        /// aus". Frueher wurde ohne TRACE gebaut, die Aufrufe fielen also komplett weg.
        /// Jetzt sind sie aktiv - ohne diese Umleitung wuerde jeder Treffer mitten im
        /// Spiel einen modalen Dialog oeffnen. Stattdessen landet er in OpenC1.log.
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
