#region Header
//
// Copyright 2003-2021 by Autodesk, Inc. 
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
//
#endregion // Header

using System;
using System.Linq;
using System.Collections;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Snoop.CollectorExts;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RevitLookup.Snoop.Collectors
{
    /// <summary>
    /// This is really a collector for any object of type System.Object.  In non .NET environments, you need
    /// multiple Collector objects to handle the fact that not everything is derived from a common class
    /// hierarchy.  In .NET, System.Object is the root of everything, so its easy to make a single Collector.
    /// </summary>
    public class CollectorObj : Collector
    {        
        static ExternalEvent externalEvent;

        public static void CreateExternalEvent()
        {
            externalEvent = ExternalEvent.Create(new ExternalEventHandler());
        }

        /// <summary>
        /// This is the point where the ball starts rolling.  We'll walk down the object's class hierarchy,
        /// continually trying to cast it to objects we know about.  NOTE: this is intentionally not Reflection.
        /// We can do that elsewhere, but here we want to explicitly control how data is formatted and navigated,
        /// so we will manually walk the entire hierarchy.
        /// </summary>
        /// <param name="obj">Object to collect data for</param>
        public Task Collect(System.Object obj)
        {
            m_dataObjs.Clear();

            if (obj == null)
                return Task.CompletedTask;

            var request  = new Request(this, obj);
            ExternalEventHandler.Queue.Enqueue(request);
            var result = externalEvent.Raise();
            return request.tcs.Task;
        }

        private class Request
        {
            public readonly TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();          
            public readonly CollectorObj Collector;
            public readonly Object ObjectToSnoop;

            public Request(CollectorObj collector, object objectToSnoop)
            {                
                this.Collector = collector;
                this.ObjectToSnoop = objectToSnoop;
            }
        }

        private class ExternalEventHandler : IExternalEventHandler
        {
            public static readonly ConcurrentQueue<Request> Queue = new ConcurrentQueue<Request>();

            public void Execute(UIApplication app)
            {
                while (Queue.TryDequeue(out Request request))
                {
                    var targetElement = request.ObjectToSnoop as Element;
                    if (request.ObjectToSnoop is IEnumerable enumerable)
                    {
                        targetElement = enumerable.OfType<Element>().FirstOrDefault();
                    }
                    Document document = targetElement?.Document;
                    Transaction transaction = document != null &&document.IsModifiable == false ? new Transaction(document, this.GetType().Name) : null;
                    transaction?.Start();

                    try
                    {
                        var collectorExtElement = new CollectorExtElement(app);
                        collectorExtElement.Collect(request.Collector, new CollectorEventArgs(request.ObjectToSnoop));
                        request.tcs.SetResult(null);
                    }
                    catch (System.Exception e)
                    {
                        request.tcs.SetException(e);
                    }
                    finally
                    {
                        transaction?.RollBack();
                    }
                }
            }        

            public string GetName()
            {
                return "RevitLookup::CollectorObj::ExternalEventHandler";
            }
        }
    }    
}