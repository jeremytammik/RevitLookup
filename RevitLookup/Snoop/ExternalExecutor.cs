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
        private static ExternalEvent _externalEvent;

        public static void CreateExternalEvent()
        {
            _externalEvent = ExternalEvent.Create(new ExternalEventHandler());
        }

        public static Task ExecuteInRevitContextAsync(Action<UIApplication> command)
        {
            var request = new Request(command);
            ExternalEventHandler.Queue.Enqueue(request);
            var result = _externalEvent.Raise();
            return request.Tcs.Task;
        }


        private class Request
        {
            public readonly TaskCompletionSource<object> Tcs = new TaskCompletionSource<object>();
            public readonly Action<UIApplication> Command;

            public Request(Action<UIApplication> command)
            {
                Command = command;
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
                        request.Command(app);
                        request.Tcs.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        request.Tcs.SetException(e);
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