#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
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
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Collectors
{
   /// <summary>
   /// This is really a collector for any object of type System.Object.  In non .NET enviroments, you need
   /// multiple Collector objects to handle the fact that not everything is derived from a common class
   /// hierarchy.  In .NET, System.Object is the root of everything, so its easy to make a single Collector.
   /// </summary>

   public class CollectorObj : Collector
   {
      // TBD: this isn't the way I wanted to do this because it isn't very extensible (as in MgdDbg),
      // but there is no static global initialization for the whole module.  So, I'm faking it here
      // by having a static on this class. (jma - 04/14/05)
      public static CollectorExts.CollectorExtElement m_colExtElement;
      public static CollectorExts.CollectorExtSymbol m_colExtSymbol;
      public static CollectorExts.CollectorExtDoc m_colExtDoc;
      public static CollectorExts.CollectorExtParams m_colExtParams;
      public static CollectorExts.CollectorExtApp m_colExtApp;
      public static CollectorExts.CollectorExtGeom m_colExtGeom;
      public static CollectorExts.CollectorExtStructural m_colExtStruct;
      public static CollectorExts.CollectorExtMisc m_colExtMisc;
      public static CollectorExts.CollectorExtSite m_colExtSite;
      public static CollectorExts.CollectorExtMEP m_colExtMEP;
      public static CollectorExts.CollectorExtEditor m_colExtEditor;
      public static CollectorExts.CollectorExtAreas m_colExtAreas;
      public static CollectorExts.CollectorExtRooms m_colExtRooms;
      public static CollectorExts.CollectorExtCreation m_colExtCreation;

      public static bool IsInitialized = false;

      public
      CollectorObj()
      {
      }

      /// <summary>
      /// This method is used to initialized static variables in this class,
      /// in .Net 4, the static variables will not be initialized until use them,
      /// so we need to call this method explicitly in App.cs when Revit Starts up
      /// </summary>
      public static void InitializeCollectors()
      {
         if (!IsInitialized)
         {
            m_colExtElement = new CollectorExts.CollectorExtElement();
            m_colExtSymbol = new CollectorExts.CollectorExtSymbol();
            m_colExtDoc = new CollectorExts.CollectorExtDoc();
            m_colExtParams = new CollectorExts.CollectorExtParams();
            m_colExtApp = new CollectorExts.CollectorExtApp();
            m_colExtGeom = new CollectorExts.CollectorExtGeom();
            m_colExtStruct = new CollectorExts.CollectorExtStructural();
            m_colExtMisc = new CollectorExts.CollectorExtMisc();
            m_colExtSite = new CollectorExts.CollectorExtSite();
            m_colExtMEP = new CollectorExts.CollectorExtMEP();
            m_colExtEditor = new CollectorExts.CollectorExtEditor();
            m_colExtAreas = new CollectorExts.CollectorExtAreas();
            m_colExtRooms = new CollectorExts.CollectorExtRooms();
            m_colExtCreation = new CollectorExts.CollectorExtCreation();
            IsInitialized = true;
            System.Diagnostics.Trace.WriteLine("Initialized");
         }
      }

      /// <summary>
      /// This is the point where the ball starts rolling.  We'll walk down the object's class hierarchy,
      /// continually trying to cast it to objects we know about.  NOTE: this is intentionally not Reflection.
      /// We can do that elsewhere, but here we want to explictly control how data is formatted and navigated,
      /// so we will manually walk the entire hierarchy.
      /// </summary>
      /// <param name="obj">Object to collect data for</param>

      public void
      Collect(System.Object obj)
      {
         m_dataObjs.Clear();

         if (obj == null)
            return;

         APIObject tmpObj = obj as APIObject;
         if (tmpObj != null)
            Stream(tmpObj);

         // now that we've collected all the data that we know about,
         // fire an event to any registered Snoop Collector Extensions so
         // they can add their data
         FireEvent_CollectExt(obj);
      }

      private void
      Stream(APIObject obj)
      {
         m_dataObjs.Add(new Snoop.Data.ClassSeparator(typeof(APIObject)));

         m_dataObjs.Add(new Snoop.Data.Bool("Is read-only", obj.IsReadOnly));

         // derived classes will get picked up by CollectorExts
      }

   }
}
