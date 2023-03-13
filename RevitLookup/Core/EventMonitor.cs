// Copyright 2003-2023 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Diagnostics;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using EventArgs = System.EventArgs;

namespace RevitLookup.Core;

public class EventMonitor
{
    private Dictionary<EventInfo, Delegate> _eventInfos;
    private Action<string, EventArgs> _handler;
    private List<string> _blockList;

    public async Task Subscribe(Action<string, EventArgs> handler)
    {
        _handler = handler;
        _eventInfos = new();
        _blockList = new List<string>(2)
        {
            nameof(UIApplication.Idling),
            nameof(Autodesk.Revit.ApplicationServices.Application.ProgressChanged)
        };
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
            {
                var name = assembly.GetName().Name;
                return name is "RevitAPI" or "RevitAPIUI";
            });

            foreach (var dll in assemblies)
            {
                foreach (var type in dll.GetTypes())
                {
                    foreach (var eventInfo in type.GetEvents())
                    {
#if DEBUG
                        Debug.WriteLine($"{eventInfo.ReflectedType} - {eventInfo.Name}");
#endif
                        if (_blockList.Contains(eventInfo.Name)) continue;
                        var target = FindValidTarget(eventInfo.ReflectedType);
                        if (target is null) break;

                        var eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, GetType().GetMethod(nameof(HandleEvent2), BindingFlags.Instance | BindingFlags
                            .Public)!);
                        await Application.AsyncEventHandler.RaiseAsync(_ => eventInfo.AddEventHandler(target, eventHandler));
                        _eventInfos.Add(eventInfo, eventHandler);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task Unsubscribe()
    {
        foreach (var eventInfo in _eventInfos)
        {
            var target = FindValidTarget(eventInfo.Key.ReflectedType);
            await Application.AsyncEventHandler.RaiseAsync(_ => eventInfo.Key.RemoveEventHandler(target, eventInfo.Value));
        }
    }

    private object FindValidTarget(Type targetType)
    {
        if (targetType == typeof(Document)) return RevitApi.Document;
        if (targetType == typeof(Autodesk.Revit.ApplicationServices.Application)) return RevitApi.Application;
        if (targetType == typeof(UIApplication)) return RevitApi.UiApplication;
        return null;
    }

    public void HandleEvent2(object sender, EventArgs args)
    {
        var stackTrace = new StackTrace();
        var stackFrames = stackTrace.GetFrames()!;
        var eventName = stackFrames[1].GetMethod().Name;
        _handler(eventName.Replace("EventHandler", ""), args);
    }
}