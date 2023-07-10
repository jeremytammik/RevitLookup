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

using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Core;

public sealed class EventMonitor
{
    private readonly Assembly[] _assemblies;
    private readonly List<string> _denyList;
    private readonly Dictionary<EventInfo, Delegate> _eventInfos = new();
    private readonly Action<string, EventArgs> _handler;

    public EventMonitor(Action<string, EventArgs> handler)
    {
        _handler = handler;
        _denyList = new List<string>(2)
        {
            nameof(UIApplication.Idling),
            nameof(Autodesk.Revit.ApplicationServices.Application.ProgressChanged)
        };

        _assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly =>
            {
                var name = assembly.GetName().Name;
                return name is "RevitAPI" or "RevitAPIUI";
            })
            .Take(2)
            .ToArray();
    }

    public void Subscribe()
    {
        Application.ActionEventHandler.Raise(Subscribe);
    }

    public void Unsubscribe()
    {
        Application.ActionEventHandler.Raise(Unsubscribe);
    }

    private void Subscribe(UIApplication uiApplication)
    {
        if (_eventInfos.Count > 0) return;

        foreach (var dll in _assemblies)
        foreach (var type in dll.GetTypes())
        foreach (var eventInfo in type.GetEvents())
        {
            Debug.Write($"RevitLookup EventMonitor: {eventInfo.ReflectedType}.{eventInfo.Name}");
            if (_denyList.Contains(eventInfo.Name)) continue;

            var targets = FindValidTargets(eventInfo.ReflectedType);
            if (targets is null)
            {
                Debug.WriteLine(" - missing target");
                break;
            }

            var methodInfo = GetType().GetMethod(nameof(OnHandlingEvent), BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)!;
            var eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);

            foreach (var target in targets) eventInfo.AddEventHandler(target, eventHandler);
            _eventInfos.Add(eventInfo, eventHandler);
            Debug.WriteLine(" - success");
        }
    }

    private void Unsubscribe(UIApplication _)
    {
        foreach (var eventInfo in _eventInfos)
        {
            var targets = FindValidTargets(eventInfo.Key.ReflectedType);
            foreach (var target in targets) eventInfo.Key.RemoveEventHandler(target, eventInfo.Value);
        }

        _eventInfos.Clear();
    }

    private static IEnumerable FindValidTargets(Type targetType)
    {
        if (targetType == typeof(Document)) return RevitApi.Application.Documents;
        if (targetType == typeof(Autodesk.Revit.ApplicationServices.Application)) return new[] {RevitApi.Application};
        if (targetType == typeof(UIApplication)) return new[] {RevitApi.UiApplication};

        return null;
    }

    [UsedImplicitly]
    public void OnHandlingEvent(object sender, EventArgs args)
    {
        var stackTrace = new StackTrace();
        var stackFrames = stackTrace.GetFrames()!;
        var eventName = stackFrames[1].GetMethod().Name;
        _handler(eventName.Replace(nameof(EventHandler), ""), args);
    }
}