using System.Collections.Concurrent;
using Autodesk.Revit.UI;

namespace RevitLookup.Core;

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
        _externalEvent.Raise();
        return request.Tcs.Task;
    }


    private class Request
    {
        public readonly Action<UIApplication> Command;
        public readonly TaskCompletionSource<object> Tcs = new();

        public Request(Action<UIApplication> command)
        {
            Command = command;
        }
    }

    private class ExternalEventHandler : IExternalEventHandler
    {
        public static readonly ConcurrentQueue<Request> Queue = new();

        public void Execute(UIApplication app)
        {
            while (Queue.TryDequeue(out var request))
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

        public string GetName()
        {
            return "RevitLookup::ExternalExecutor::ExternalEventHandler";
        }
    }
}