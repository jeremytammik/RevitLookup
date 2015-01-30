#region Header
//
// Copyright 2003-2015 by Autodesk, Inc. 
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
using System.Collections;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
   /// <summary>
   /// Provide Snoop.Data for any classes related to Structural.
   /// </summary>

   public class CollectorExtStructural : CollectorExt
   {
      public
      CollectorExtStructural()
      {
      }

      protected override void
      CollectEvent(object sender, CollectorEventArgs e)
      {
         // cast the sender object to the SnoopCollector we are expecting
         Collector snoopCollector = sender as Collector;
         if (snoopCollector == null)
         {
            Debug.Assert(false);    // why did someone else send us the message?
            return;
         }

         // see if it is a type we are responsible for
         AnalyticalModel aModel = e.ObjToSnoop as AnalyticalModel;
         if (aModel != null)
         {
            Stream(snoopCollector.Data(), aModel);
            return;
         }

         CompoundStructure compStruct = e.ObjToSnoop as CompoundStructure;
         if (compStruct != null)
         {
            Stream(snoopCollector.Data(), compStruct);
            return;
         }

         CompoundStructureLayer compStructLayer = e.ObjToSnoop as CompoundStructureLayer;
         if (compStructLayer != null)
         {
            Stream(snoopCollector.Data(), compStructLayer);
            return;
         }

         AnalyticalModelSupport supportData = e.ObjToSnoop as AnalyticalModelSupport;
         if (supportData != null)
         {
            Stream(snoopCollector.Data(), supportData);
            return;
         }

         RebarInSystem barDesc = e.ObjToSnoop as RebarInSystem;
         if (barDesc != null)
         {
            Stream(snoopCollector.Data(), barDesc);
            return;
         }
      }

      private void
      Stream(ArrayList data, AnalyticalModel aModel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModel)));

         try {
            data.Add(new Snoop.Data.Object("GetCurve", aModel.GetCurve()));
         }
         catch (System.Exception ex){
            data.Add(new Snoop.Data.Exception("GetCurve", ex));
         }
         data.Add(new Snoop.Data.Enumerable("GetCurves", aModel.GetCurves(AnalyticalCurveType.ActiveCurves)));
         try {
            data.Add(new Snoop.Data.Xyz("GetPoint", aModel.GetPoint()));
         }
         catch (System.Exception ex){
            data.Add(new Snoop.Data.Exception("GetPoint", ex));
         }
         data.Add(new Snoop.Data.Enumerable("GetAnalyticalModelSupports", aModel.GetAnalyticalModelSupports()));
      }


      private void
      Stream(ArrayList data, CompoundStructure compStruct)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CompoundStructure)));

         data.Add(new Snoop.Data.Enumerable("Layers", compStruct.GetLayers()));
      }

      private void
      Stream(ArrayList data, CompoundStructureLayer compStructLayer)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CompoundStructureLayer)));

         data.Add(new Snoop.Data.String("Function", compStructLayer.Function.ToString()));
         data.Add(new Snoop.Data.Object("MaterialId", GetElementById(compStructLayer.MaterialId)));
         data.Add(new Snoop.Data.Double("Width", compStructLayer.Width));
         data.Add(new Snoop.Data.Object("DeckProfileId", GetElementById(compStructLayer.DeckProfileId)));
         //data.Add(new Snoop.Data.String("Deck usage", compStructLayer.DeckUsage.ToString()));
         //data.Add(new Snoop.Data.Bool("Variable", compStructLayer.));
      }

      private void Stream(ArrayList data, AnalyticalModelSupport supportData)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModelSupport)));

         //supportData.GetSupportType()
         data.Add(new Snoop.Data.String("SupportType", supportData.GetSupportType().ToString()));
         data.Add(new Snoop.Data.Xyz("Point", supportData.GetPoint()));
         data.Add(new Snoop.Data.String("Priority", supportData.GetPriority().ToString()));
      }

      private void
      Stream(ArrayList data, RebarInSystem barDesc)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarInSystem)));

         data.Add(new Snoop.Data.Object("Bar type", GetElementById(barDesc.GetTypeId())));
         data.Add(new Snoop.Data.Object("Bar shape", GetElementById(barDesc.RebarShapeId)));
         //data.Add(new Snoop.Data.Bool("Hooks in same direction", barDesc.HooksInSameDirection));
         //data.Add(new Snoop.Data.Int("Layer", barDesc.Layer));
         data.Add(new Snoop.Data.Double("Length", barDesc.TotalLength));

         //data.Add(new Snoop.Data.CategorySeparator("Hook Types"));
         //data.Add(new Snoop.Data.Int("Count", barDesc.Count));

         data.Add(new Snoop.Data.Object("Start hook type", barDesc.GetHookTypeId(0)));
         data.Add(new Snoop.Data.Object("End hook type", barDesc.GetHookTypeId(1)));
      }
   }
}
