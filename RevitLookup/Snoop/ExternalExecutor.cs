using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitLookup.Snoop
{
    internal static class ExternalExecutor
    {
        private static ExternalEvent externalEvent;

        public static void CreateExternalEvent()
        {
            externalEvent = ExternalEvent.Create(new ExternalEventHandler());
        }

        public static Task ExecuteInRevitContextAsync(Action<UIApplication> command)
        {
            var request = new Request(command);
            ExternalEventHandler.Queue.Enqueue(request);
            var result = externalEvent.Raise();
            return request.tcs.Task;
        }


        private class Request
        {
            public readonly TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            public readonly Action<UIApplication> command;

            public Request(Action<UIApplication> command)
            {
                this.command = command;
            }
        }

        private class ExternalEventHandler : IExternalEventHandler
        {
            public static readonly ConcurrentQueue<Request> Queue = new ConcurrentQueue<Request>();

            public void Execute(UIApplication app)
            {
                while (Queue.TryDequeue(out Request request))
                {
                    try
                    {
                        request.command(app);
                        request.tcs.SetResult(null);
                    }
                    catch (System.Exception e)
                    {
                        request.tcs.SetException(e);
                    }
                }
            }

            public string GetName()
            {
                return "RevitLookup::ExternalExecutor::ExternalEventHandler";
            }
        }
    }
}