#region Header
//
// Copyright 2003-2016 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace RevitLookup.Test
{

   class TestDocument : RevitLookupTestFuncs
   {

      public
      TestDocument(Autodesk.Revit.UI.UIApplication app)
         : base(app)
      {
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Delete SelSet", "Delete the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Delete), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Move SelSet --> (10', 10', 0')", "Move the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Move), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Rotate SelSet by 45 degrees", "Rotate the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Rotate), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Load Family", "Load a .rfa file", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(LoadFamily), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Linear Array SelSet (Number = 4)", "Linearly array the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(LinearArray), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Radial Array SelSet by 30 degrees (Number = 4)", "Radially array the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(RadialArray), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Linear Array SelSet without associate", "Linearly array the current SelSet without associate", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(ArrayWithoutAssociate), RevitLookupTestFuncInfo.TestType.Modify));
         //m_testFuncs.Add(new RevitLookupTestFuncInfo("Mirror", "Mirror current SelSet", typeof(Revit.Document), new RevitLookupTestFuncInfo.TestFunc(Mirror), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Boundary Lines", "Draw lines to sketch the Room Boundary", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(SketchBoundary), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Room Area", "Insert Area Value textnotes for all available rooms", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(RoomArea), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Filter Element types", "Filter for selected element types", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(FilterElementTypes), RevitLookupTestFuncInfo.TestType.Query));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Add family types and parameters", "Use family manager for adddition of types/params", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(AddFamilyParameterAndType), RevitLookupTestFuncInfo.TestType.Create));
      }

      public void
      Delete()
      {
         m_revitApp.ActiveUIDocument.Document.Delete(m_revitApp.ActiveUIDocument.Selection.GetElementIds());
      }

      public void
      Move()
      {
         XYZ vec = new XYZ(10.0, 10.0, 0.0);
         ElementTransformUtils.MoveElements(
            m_revitApp.ActiveUIDocument.Document,
            m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
            vec
            );
      }

      public void
      Rotate()
      {
          Line zAxis = Line.CreateUnbound(GeomUtils.kOrigin, GeomUtils.kZAxis);
         ElementTransformUtils.RotateElements(
            m_revitApp.ActiveUIDocument.Document,
            m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
            zAxis, GeomUtils.kRad45
            );
      }

      public void
      LoadFamily()
      {
         Utils.UserInput.LoadFamily(null, m_revitApp.ActiveUIDocument.Document);
      }

      public void
      LinearArray()
      {
         if (m_revitApp.ActiveUIDocument.Selection.GetElementIds().Count > 0)
         {
            Autodesk.Revit.DB.LinearArray.Create(
               m_revitApp.ActiveUIDocument.Document,
               m_revitApp.ActiveUIDocument.Document.ActiveView,
               m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
               4, GeomUtils.kYAxis, ArrayAnchorMember.Last);
         }
      }

      public void
      RadialArray()
      {
         if (m_revitApp.ActiveUIDocument.Selection.GetElementIds().Count > 0)
         {
            Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;
            XYZ axisDir = null;
            if (view is View3D)
            {
               SketchPlane sp = view.SketchPlane;
               if (sp == null)
                  return;
               axisDir = sp.GetPlane().Normal;
            }
            else
               axisDir = view.ViewDirection;
            if (axisDir == null)
               return;
            Line axisLine = Line.CreateBound(XYZ.Zero, XYZ.Zero.Add(axisDir));
            Autodesk.Revit.DB.RadialArray.Create(
               m_revitApp.ActiveUIDocument.Document,
               m_revitApp.ActiveUIDocument.Document.ActiveView,
               m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
               4, axisLine, 30.0, ArrayAnchorMember.Last);
         }
      }

      public void
      ArrayWithoutAssociate()
      {
         if (m_revitApp.ActiveUIDocument.Selection.GetElementIds().Count > 0)
         {
            Autodesk.Revit.DB.LinearArray.ArrayElementsWithoutAssociation(
               m_revitApp.ActiveUIDocument.Document,
               m_revitApp.ActiveUIDocument.Document.ActiveView,
               m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
               4, GeomUtils.kYAxis, ArrayAnchorMember.Second);
         }
      }

      public void
      Mirror()
      {   // TBD: 

         //XYZ startPt1 = new XYZ(0.0, 0.0, 0.0);
         //XYZ endPt1 = new XYZ(56.5, 0.0, 0.0);

         //Line line1 = m_revitApp.Create.NewLine(ref startPt1, ref endPt1, true);

         ////// Note: There seems to be an issue with the Reference returned by curves. It always remains null.
         ////// arj 1/12/07           
         //m_revitApp.ActiveUIDocument.Document.Mirror(m_revitApp.ActiveUIDocument.Selection.Elements, line1.Reference);
      }


      /// <summary>
      /// Draw lines to sketch the boundaries of available rooms in the Active Document.
      /// </summary>
      public void
      SketchBoundary()
      {
         Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
         SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);

         RevitLookup.Test.Forms.Levels lev = new RevitLookup.Test.Forms.Levels(m_revitApp);
         if (lev.ShowDialog() != DialogResult.OK)
            return;

         Level curLevel = lev.LevelSelected;
         if (curLevel == null)
         {
            MessageBox.Show("No Level was selected.");
            return;
         }

         // Get the plan topology of the active doc first
         PlanTopology planTopo = m_revitApp.ActiveUIDocument.Document.get_PlanTopology(curLevel);
         ICollection<ElementId> roomIds = planTopo.GetRoomIds();

         if (roomIds.Count > 0)
         {
            IEnumerator<ElementId> setIter = roomIds.GetEnumerator();
            while (setIter.MoveNext())
            {
               Autodesk.Revit.DB.Architecture.Room room = m_revitApp.ActiveUIDocument.Document.GetElement(setIter.Current) as Autodesk.Revit.DB.Architecture.Room;

               if (null != room)
               {
                  IList<IList<Autodesk.Revit.DB.BoundarySegment>> boundSegArrayArray = room.GetBoundarySegments(new SpatialElementBoundaryOptions());

                  foreach (IList<Autodesk.Revit.DB.BoundarySegment> boundSegArray in boundSegArrayArray)
                  {
                     foreach (Autodesk.Revit.DB.BoundarySegment boundSeg in boundSegArray)
                     {
                        if (null != boundSeg)
                        {
                           // once you get to the Boundary Segment which represent one of the sides of the room boundary, draw a Model Curve to 
                           // represent the outline.
                           ModelCurve modCurve = m_revitApp.ActiveUIDocument.Document.Create.NewModelCurve(boundSeg.GetCurve(), sketchPlane);
                        }
                     }
                  }
               }
            }
         }
         else
         {
            MessageBox.Show("No rooms found in the Active Document", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }

      }


      /// <summary>
      /// Calulate the area of all the available rooms and specify them using TextNotes
      /// </summary>
      public void
      RoomArea()
      {
         Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
         SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);

         RevitLookup.Test.Forms.Levels lev = new RevitLookup.Test.Forms.Levels(m_revitApp);
         if (lev.ShowDialog() != DialogResult.OK)
            return;

         Level curLevel = lev.LevelSelected;
         if (curLevel == null)
         {
            MessageBox.Show("No Level was selected.");
            return;
         }

         // Get the plan topology of the active doc first
         PlanTopology planTopo = m_revitApp.ActiveUIDocument.Document.get_PlanTopology(curLevel);
         ICollection<ElementId> roomIds = planTopo.GetRoomIds();

         if (roomIds.Count > 0)
         {
            IEnumerator<ElementId> setIter = roomIds.GetEnumerator();
            while (setIter.MoveNext())
            {
               Room room = m_revitApp.ActiveUIDocument.Document.GetElement(setIter.Current) as Room;
               if (null != room)
               {
                  Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;
                  LocationPoint locationPoint = room.Location as LocationPoint;

                  Double area = room.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble();

                  Double roundedArea = Math.Round(area, 2);

                  // providing an offset so that the Room Tag and the Area Tag dont overlap. Overlapping leads to an 
                  // alignment related assert.

                  XYZ offset = new XYZ(5.0, 0, 0);

                  /// align text middle and center
                  TextNoteOptions op = new TextNoteOptions();
                  TextNote txtNote = TextNote.Create(m_revitApp.ActiveUIDocument.Document, view.Id, offset + locationPoint.Point, .25, roundedArea.ToString(), op);
               }
            }
         }
         else
         {
            MessageBox.Show("No rooms found in the Active Document", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }


         // TBD: Tried to play around with PlanCircuits and there seems to be a problem with the IsRoomLocated property. 
         // arj 1/23/07

         //Revit.PlanCircuitSet circSet = planTopo.Circuits;
         //Revit.PlanCircuitSetIterator setIters = circSet.ForwardIterator();

         //while (setIters.MoveNext())
         //{
         //    Revit.PlanCircuit planCircuit = setIters.Current as Revit.PlanCircuit;

         //    if (null != planCircuit)
         //    {
         //
         //        if (planCircuit.IsRoomLocated) // throws an exception always "Attempted to read or write protected memory.
         // This is often an indication that other memory is corrupt."
         //        {
         //        }
         //    }
         //}
      }

      /// <summary>
      /// Have the user select a type of Element and then filter the document for all instances of that type.
      /// </summary>
      public void FilterElementTypes()
      {
         Test.Forms.Elements elems = new Test.Forms.Elements(m_revitApp);
         if (elems.ShowDialog() != DialogResult.OK)
            return;

         ElementSet elemSet = new ElementSet();

         FilteredElementCollector fec = new FilteredElementCollector(m_revitApp.ActiveUIDocument.Document);
         ElementClassFilter whatAreWanted = new ElementClassFilter(elems.ElemTypeSelected);
         fec.WherePasses(whatAreWanted);
         List<Element> elements = fec.ToElements() as List<Element>;

         foreach (Element element in elements)
         {
            elemSet.Insert(element);
         }

         Snoop.Forms.Objects objs = new Snoop.Forms.Objects(elemSet);
         objs.ShowDialog();
      }

      /// <summary>
      /// Ask the user to open a revit family template and then add FamilyTypes and FamilyParameters
      /// to it. Say the user opens a Door template, he can then save the family as a new door family and load
      /// it into a new project for use.
      /// </summary>
      public void AddFamilyParameterAndType()
      {
         Document doc;

         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.Title = "Select family document";
         openFileDialog.Filter = "RFT Files (*.rft)|*.rft";

         if (openFileDialog.ShowDialog() == DialogResult.OK)
         {
            doc = m_revitApp.Application.NewFamilyDocument(openFileDialog.FileName);
         }
         else
            return;

         using( Transaction transaction = new Transaction( m_revitApp.ActiveUIDocument.Document ) )
         {
           transaction.Start( "AddFamilyParameterAndType" );

           if( doc.IsFamilyDocument )
           { // has to be a family document to be able to use the Family Manager.

             FamilyManager famMgr = doc.FamilyManager;

             //Add a family param. 
             FamilyParameter famParam = famMgr.AddParameter( "RevitLookup_Param", BuiltInParameterGroup.PG_TITLE, ParameterType.Text, false );
             famMgr.Set( famParam, "Default text." );

             //Create a couple of new family types. Note that we can set different values for the param
             //in different family types.                

             FamilyType newFamType = famMgr.NewType( "RevitLookup_Type1" );
             famMgr.CurrentType = newFamType;

             if( newFamType.HasValue( famParam ) )
             {
               famMgr.Set( famParam, "Text1." );
             }

             FamilyType newFamType1 = famMgr.NewType( "RevitLookup_Type2" );
             famMgr.CurrentType = newFamType;

             if( newFamType.HasValue( famParam ) )
             {
               famMgr.Set( famParam, "Text2." );
             }

             famMgr.MakeType( famParam );

             if( ( famParam != null ) && ( newFamType != null ) )
             {
               MessageBox.Show( "New family types/params added successfully.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information );
             }
             else
               MessageBox.Show( "Family types/params addition failed.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Error );
           }

           transaction.Commit();
         }
      }
   }
}
