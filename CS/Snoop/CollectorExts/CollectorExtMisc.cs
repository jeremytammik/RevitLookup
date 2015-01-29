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
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Mechanical;


using RevitLookup.Snoop.Collectors;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Reflection;

namespace RevitLookup.Snoop.CollectorExts
{

   public class CollectorExtMisc : CollectorExt
   {
      public
      CollectorExtMisc()
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
         Color color = e.ObjToSnoop as Color;
         if (color != null)
         {
            Stream(snoopCollector.Data(), color);
            return;
         }

         LayoutRule layoutRule = e.ObjToSnoop as LayoutRule;
         if (layoutRule != null)
         {
            Stream(snoopCollector.Data(), layoutRule);
            return;
         }

         FormatOptions formatOptions = e.ObjToSnoop as FormatOptions;
         if (formatOptions != null)
         {
            Stream(snoopCollector.Data(), formatOptions);
            return;
         }

         CurtainGrid curtainGrid = e.ObjToSnoop as CurtainGrid;
         if (curtainGrid != null)
         {
            Stream(snoopCollector.Data(), curtainGrid);
            return;
         }

         CurtainCell curtainCell = e.ObjToSnoop as CurtainCell;
         if (curtainCell != null)
         {
            Stream(snoopCollector.Data(), curtainCell);
            return;
         }

         RebarHostData rebarHostData = e.ObjToSnoop as RebarHostData;
         if (rebarHostData != null)
         {
            Stream(snoopCollector.Data(), rebarHostData);
            return;
         }

         Leader leader = e.ObjToSnoop as Leader;
         if (leader != null)
         {
            Stream(snoopCollector.Data(), leader);
            return;
         }

         AreaVolumeSettings areaSettings = e.ObjToSnoop as AreaVolumeSettings;
         if (areaSettings != null)
         {
            Stream(snoopCollector.Data(), areaSettings);
            return;
         }

         ViewSheetSetting viewSheetSetting = e.ObjToSnoop as ViewSheetSetting;
         if (viewSheetSetting != null)
         {
            Stream(snoopCollector.Data(), viewSheetSetting);
            return;
         }

         Autodesk.Revit.UI.Events.DialogBoxData dlgBoxData = e.ObjToSnoop as Autodesk.Revit.UI.Events.DialogBoxData;
         if (dlgBoxData != null)
         {
            Stream(snoopCollector.Data(), dlgBoxData);
            return;
         }

         Construction construct = e.ObjToSnoop as Construction;
         if (construct != null)
         {
            Stream(snoopCollector.Data(), construct);
            return;
         }

         FamilyElementVisibility famElemVisib = e.ObjToSnoop as FamilyElementVisibility;
         if (famElemVisib != null)
         {
            Stream(snoopCollector.Data(), famElemVisib);
            return;
         }

         FamilyManager famManager = e.ObjToSnoop as FamilyManager;
         if (famManager != null)
         {
            Stream(snoopCollector.Data(), famManager);
            return;
         }

         FamilyParameter famParam = e.ObjToSnoop as FamilyParameter;
         if (famParam != null)
         {
            Stream(snoopCollector.Data(), famParam);
            return;
         }

         FamilyType famType = e.ObjToSnoop as FamilyType;
         if (famType != null)
         {
            Stream(snoopCollector.Data(), famType);
            return;
         }

         MEPSpaceConstruction mepSpaceConstuct = e.ObjToSnoop as MEPSpaceConstruction;
         if (mepSpaceConstuct != null)
         {
            Stream(snoopCollector.Data(), mepSpaceConstuct);
            return;
         }

         BuildingSiteExportOptions bldSiteExpOptions = e.ObjToSnoop as BuildingSiteExportOptions;
         if (bldSiteExpOptions != null)
         {
            Stream(snoopCollector.Data(), bldSiteExpOptions);
            return;
         }

         DGNExportOptions dgnExpOptions = e.ObjToSnoop as DGNExportOptions;
         if (dgnExpOptions != null)
         {
            Stream(snoopCollector.Data(), dgnExpOptions);
            return;
         }

         DWFExportOptions dwfExpOptions = e.ObjToSnoop as DWFExportOptions;
         if (dwfExpOptions != null)
         {
            Stream(snoopCollector.Data(), dwfExpOptions);
            return;
         }

         DWGExportOptions dwgExpOptions = e.ObjToSnoop as DWGExportOptions;
         if (dwgExpOptions != null)
         {
            Stream(snoopCollector.Data(), dwgExpOptions);
            return;
         }

         DWGImportOptions dwgImpOptions = e.ObjToSnoop as DWGImportOptions;
         if (dwgImpOptions != null)
         {
            Stream(snoopCollector.Data(), dwgImpOptions);
            return;
         }

         FBXExportOptions fbxExpOptions = e.ObjToSnoop as FBXExportOptions;
         if (fbxExpOptions != null)
         {
            Stream(snoopCollector.Data(), fbxExpOptions);
            return;
         }

         TrussMemberInfo trussInfo = e.ObjToSnoop as TrussMemberInfo;
         if (trussInfo != null)
         {
            Stream(snoopCollector.Data(), trussInfo);
            return;
         }

         VertexIndexPair vertIndPair = e.ObjToSnoop as VertexIndexPair;
         if (vertIndPair != null)
         {
            Stream(snoopCollector.Data(), vertIndPair);
            return;
         }

         PointElementReference ptElemRef = e.ObjToSnoop as PointElementReference;
         if (ptElemRef != null)
         {
            Stream(snoopCollector.Data(), ptElemRef);
            return;
         }

         Autodesk.Revit.DB.Architecture.BoundarySegment boundSeg = e.ObjToSnoop as Autodesk.Revit.DB.Architecture.BoundarySegment;
         if (boundSeg != null)
         {
            Stream(snoopCollector.Data(), boundSeg);
            return;
         }

         PointLocationOnCurve ptLocOnCurve = e.ObjToSnoop as PointLocationOnCurve;
         if (ptLocOnCurve != null)
         {
            Stream(snoopCollector.Data(), ptLocOnCurve);
            return;
         }

         Entity entity = e.ObjToSnoop as Entity;
         if (entity != null)
         {
            Stream(snoopCollector.Data(), entity);
            return;
         }

         Field field = e.ObjToSnoop as Field;
         if (field != null)
         {
            Stream(snoopCollector.Data(), field);
            return;
         }

         ExtensibleStorageField storeagefield = e.ObjToSnoop as ExtensibleStorageField;
         if (storeagefield != null)
         {
            Stream(snoopCollector.Data(), storeagefield);
            return;
         }

         IList<Autodesk.Revit.DB.BoundarySegment> boundSegs = e.ObjToSnoop as
            IList<Autodesk.Revit.DB.BoundarySegment>;
         if (boundSegs != null)
         {
            Stream(snoopCollector.Data(), boundSegs);
            return;
         }

         if (e.ObjToSnoop is KeyValuePair<String, String>)
         {
            KeyValuePair<String, String> stringspair = (KeyValuePair<String, String>)e.ObjToSnoop;
            Stream(snoopCollector.Data(), stringspair);
            return;
         }

         Schema schema = e.ObjToSnoop as Schema;
         if (schema != null)
         {
            Stream(snoopCollector.Data(), schema);
            return;
         }

         ElementId elemId = e.ObjToSnoop as ElementId;
         if (elemId != null)
         {
            Stream(snoopCollector.Data(), elemId);
            return;
         }

         PlanViewRange plvr = e.ObjToSnoop as PlanViewRange;
         if (plvr != null)
         {
             Stream(snoopCollector.Data(), plvr);
             return;
         }
         //TF
         RebarConstraintsManager rbcm = e.ObjToSnoop as RebarConstraintsManager;
         if (rbcm != null)
         {
             Stream(snoopCollector.Data(), rbcm);
             return;
         }

         RebarConstrainedHandle rbch = e.ObjToSnoop as RebarConstrainedHandle;
         if (rbch != null)
         {
             Stream(snoopCollector.Data(), rbch);
             return;
         }

         RebarConstraint rbc = e.ObjToSnoop as RebarConstraint;
         if (rbc != null)
         {
             Stream(snoopCollector.Data(), rbc);
             return;
         }

         //TFEND

         if (Utils.IsSupportedType(e.ObjToSnoop) && e.ObjToSnoop != null)
            Utils.StreamWithReflection(snoopCollector.Data(), e.ObjToSnoop.GetType(), e.ObjToSnoop);
      }

      private void
      Stream(ArrayList data, ElementId elemId)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Schema)));
         Utils.StreamWithReflection(data, typeof(ElementId), elemId);
      }

      class ExtensibleStorageField
      {
         public ExtensibleStorageField(Entity entity, Field field)
         {
            _entity = entity;
            _field = field;
         }

         public Field Field
         {
            get{return _field;}
         }

         public Entity Entity
         {
            get{return _entity;}
         }

         private Entity _entity;
         private Field _field;
      }

      private void
      Stream(ArrayList data, PlanViewRange plvr)
      {
          data.Add(new Snoop.Data.ClassSeparator(typeof(PlanViewRange)));

          data.Add(new Snoop.Data.ElementId("Level Id for CutPlane", plvr.GetLevelId(PlanViewPlane.CutPlane), m_app.ActiveUIDocument.Document));
          data.Add(new Snoop.Data.Double("Offset assiciated with CutPlane", plvr.GetOffset(PlanViewPlane.CutPlane)));
          data.Add(new Snoop.Data.ElementId("Level Id for Bottom Clib Plane", plvr.GetLevelId(PlanViewPlane.BottomClipPlane), m_app.ActiveUIDocument.Document));
          data.Add(new Snoop.Data.Double("Offset assiciated with Bottom Clib Plane", plvr.GetOffset(PlanViewPlane.BottomClipPlane)));
          data.Add(new Snoop.Data.ElementId("Level Id for Top Clib Plane", plvr.GetLevelId(PlanViewPlane.TopClipPlane), m_app.ActiveUIDocument.Document));
          data.Add(new Snoop.Data.Double("Offset assiciated with Top Clib Plane", plvr.GetOffset(PlanViewPlane.TopClipPlane)));
          data.Add(new Snoop.Data.ElementId("Level Id for Underlay Bottom", plvr.GetLevelId(PlanViewPlane.UnderlayBottom), m_app.ActiveUIDocument.Document));
          data.Add(new Snoop.Data.Double("Offset assiciated with Underlay Bottom Plane", plvr.GetOffset(PlanViewPlane.UnderlayBottom)));
          data.Add(new Snoop.Data.ElementId("Level Id for View Depth Plane", plvr.GetLevelId(PlanViewPlane.ViewDepthPlane), m_app.ActiveUIDocument.Document));
          data.Add(new Snoop.Data.Double("Offset assiciated with View Depth Plane", plvr.GetOffset(PlanViewPlane.ViewDepthPlane)));
      }

      private void
      Stream(ArrayList data, Schema schema)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Schema)));
         data.Add(new Snoop.Data.String("SchemaName", schema.SchemaName));
         data.Add(new Snoop.Data.String("Vendor Id", schema.VendorId));
         data.Add(new Snoop.Data.String("Application GUID", schema.ApplicationGUID.ToString()));
         data.Add(new Snoop.Data.String("Documentation", schema.Documentation));
         data.Add(new Snoop.Data.String("GUID", schema.GUID.ToString()));
         data.Add(new Snoop.Data.String("Read Access Level", schema.ReadAccessLevel.ToString()));
         data.Add(new Snoop.Data.String("Write Access Level", schema.WriteAccessLevel.ToString()));
         data.Add(new Snoop.Data.Object("Fields", schema.ListFields()));
      }


      private void
      Stream(ArrayList data, ExtensibleStorageField storageField)
      {
         Field field = storageField.Field;
         Entity entity = storageField.Entity;
         data.Add(new Snoop.Data.ClassSeparator(typeof(Field)));
         data.Add(new Snoop.Data.String("Container Type", field.ContainerType.ToString()));
         data.Add(new Snoop.Data.String("Documentation", field.Documentation));
         if(field.KeyType != null)
            data.Add(new Snoop.Data.String("Key Type", field.KeyType.ToString()));
         data.Add(new Snoop.Data.String("Value Type", field.ValueType.ToString()));
         data.Add(new Snoop.Data.String("Unit Type", field.UnitType.ToString()));

         Schema schema = field.Schema;
         if (schema != null)
         {
            data.Add(new Snoop.Data.Object("Schema", schema));

            Object value = null;
            DisplayUnitType dut = DisplayUnitType.DUT_UNDEFINED;
            
            Type[] typeParameters = new Type[2] { typeof(Field), 
                  typeof(Autodesk.Revit.DB.DisplayUnitType)};
            MethodInfo methodInfo = typeof(Entity).GetMethod("Get", typeParameters);

            // try to get a valid display unit type here.
            if (field.UnitType != UnitType.UT_Undefined)
               dut = m_activeDoc.GetUnits().GetFormatOptions(field.UnitType).DisplayUnits;

            switch (field.ContainerType)
            {

                case ContainerType.Simple:
                    {

                        //MethodInfo genericGet = methodInfo.MakeGenericMethod(field.ValueType);
                        MethodInfo genericGet = methodInfo.MakeGenericMethod(new Type[] { field.ValueType });
                        Object[] parameters = new Object[2] { field, dut };
                        value = genericGet.Invoke(entity, parameters);
                        break;
                    }
                case ContainerType.Array:
                    {
                        String typeName = typeof(IList<int>).AssemblyQualifiedName.Replace(typeof(int).AssemblyQualifiedName,
                          field.ValueType.AssemblyQualifiedName);
                        Type type = Type.GetType(typeName);
                        //MethodInfo genericGet = methodInfo.MakeGenericMethod(type);
                        MethodInfo genericGet = methodInfo.MakeGenericMethod(new Type[] { type });
                        Object[] parameters = new Object[2] { field, dut };
                        value = genericGet.Invoke(entity, parameters);
                        break;
                    }
                case ContainerType.Map:
                    {
                        //String typeName = typeof(IDictionary<int, double>).AssemblyQualifiedName.Replace(
                        //  typeof(int).AssemblyQualifiedName, field.KeyType.ToString()).Replace(
                        //  typeof(double).AssemblyQualifiedName, field.ValueType.ToString());
                        String typeName = typeof(IDictionary<int, double>).AssemblyQualifiedName.Replace(
                          typeof(int).AssemblyQualifiedName, field.KeyType.AssemblyQualifiedName).Replace(
                          typeof(double).AssemblyQualifiedName, field.ValueType.AssemblyQualifiedName);
                        Type type = Type.GetType(typeName);
                        //MethodInfo genericGet = methodInfo.MakeGenericMethod(type);
                        MethodInfo genericGet = methodInfo.MakeGenericMethod(new Type[] { type });
                        Object[] parameters = new Object[2] { field, dut };
                        value = genericGet.Invoke(entity, parameters);
                        break;
                    }
                default:
                    break;
            }
            data.Add(new Snoop.Data.String("Display Unit Type", dut.ToString()));
            Utils.AppendThePropertyObject(data, field.FieldName, value);
         }

      }

      private void
      Stream(ArrayList data, Entity entity)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Entity)));
         data.Add(new Snoop.Data.Bool("Is Valid", entity.IsValid()));
         data.Add(new Snoop.Data.Bool("Read Access Granted", entity.ReadAccessGranted()));
         data.Add(new Snoop.Data.Bool("Write Access Granted", entity.WriteAccessGranted()));
         data.Add(new Snoop.Data.Object("Schema", entity.Schema));
         Schema schema = entity.Schema;
         if (schema != null)
         {
            foreach (Field field in schema.ListFields())
            {
               data.Add(new Snoop.Data.Object("Field", new ExtensibleStorageField(entity, field)));
            }
         }

      }

      private void
      Stream(ArrayList data, Field field)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Field)));
         data.Add(new Snoop.Data.ClassSeparator(typeof(Field)));
         data.Add(new Snoop.Data.String("Container Type", field.ContainerType.ToString()));
         data.Add(new Snoop.Data.String("Documentation", field.Documentation));
         if (field.KeyType != null)
            data.Add(new Snoop.Data.String("Key Type", field.KeyType.ToString()));
         data.Add(new Snoop.Data.String("Value Type", field.ValueType.ToString()));
         data.Add(new Snoop.Data.String("Unit Type", field.UnitType.ToString()));
         data.Add(new Snoop.Data.Object("Schema", field.Schema));
         data.Add(new Snoop.Data.Object("Sub Schema", field.SubSchema));
         data.Add(new Snoop.Data.String("Sub Schema GUID", field.SubSchemaGUID.ToString()));
      }

      private void
      Stream(ArrayList data, IList<Autodesk.Revit.DB.BoundarySegment> boundSegs)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(IList<Autodesk.Revit.DB.BoundarySegment>)));
         foreach(Autodesk.Revit.DB.BoundarySegment boundSeg in boundSegs)
            data.Add(new Snoop.Data.Object("Boundary Segment", boundSeg));
      }

      private void
      Stream(ArrayList data, KeyValuePair<String, String> stringPair)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(KeyValuePair<String, String>)));
         data.Add(new Snoop.Data.String(stringPair.Key, stringPair.Value));
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Color color)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Color)));

         data.Add(new Snoop.Data.Int("Red", color.Red));
         data.Add(new Snoop.Data.Int("Green", color.Green));
         data.Add(new Snoop.Data.Int("Blue", color.Blue));
      }

      private void
      Stream(ArrayList data, LayoutRule layoutRule)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LayoutRule)));

         // no data at this level

         LayoutRuleClearSpacing lrClearSpacing = layoutRule as LayoutRuleClearSpacing;
         if (lrClearSpacing != null)
         {
            Stream(data, lrClearSpacing);
            return;
         }

         LayoutRuleFixedDistance lrFixedDist = layoutRule as LayoutRuleFixedDistance;
         if (lrFixedDist != null)
         {
            Stream(data, lrFixedDist);
            return;
         }

         LayoutRuleFixedNumber lrFixedNum = layoutRule as LayoutRuleFixedNumber;
         if (lrFixedNum != null)
         {
            Stream(data, lrFixedNum);
            return;
         }

         LayoutRuleMaximumSpacing lrMaxSpacing = layoutRule as LayoutRuleMaximumSpacing;
         if (lrMaxSpacing != null)
         {
            Stream(data, lrMaxSpacing);
            return;
         }
      }

      private void
      Stream(ArrayList data, LayoutRuleClearSpacing layoutRule)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LayoutRuleClearSpacing)));

         data.Add(new Snoop.Data.String("Justify type", layoutRule.JustifyType.ToString()));
         data.Add(new Snoop.Data.Double("Spacing", layoutRule.Spacing));
      }

      private void
      Stream(ArrayList data, LayoutRuleFixedDistance layoutRule)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LayoutRuleFixedDistance)));

         data.Add(new Snoop.Data.String("Justify type", layoutRule.JustifyType.ToString()));
         data.Add(new Snoop.Data.Double("Spacing", layoutRule.Spacing));
      }

      private void
      Stream(ArrayList data, LayoutRuleFixedNumber layoutRule)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LayoutRuleFixedNumber)));

         data.Add(new Snoop.Data.Int("Number of lines", layoutRule.NumberOfLines));
      }

      private void
      Stream(ArrayList data, LayoutRuleMaximumSpacing layoutRule)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LayoutRuleMaximumSpacing)));

         data.Add(new Snoop.Data.Double("Spacing", layoutRule.Spacing));
      }

      private void
      Stream(ArrayList data, FormatOptions opt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FormatOptions)));

         //data.Add( new Snoop.Data.String( "Name", opt.GetName() ) ); // 2015, jeremy: 'Autodesk.Revit.DB.FormatOptions.GetName()' is obsolete: 'This method is deprecated in Revit 2015.  Use UnitUtils.GetTypeCatalogString(DisplayUnitType) instead.'
         data.Add( new Snoop.Data.String( "Name", UnitUtils.GetTypeCatalogString(opt.DisplayUnits) ) ); // 2016

 		 data.Add(new Snoop.Data.Bool("Use default", opt.UseDefault));
         if (!opt.UseDefault)
         {
            data.Add(new Snoop.Data.String("Units", opt.DisplayUnits.ToString()));
            data.Add(new Snoop.Data.String("Unit symbol", opt.UnitSymbol.ToString()));
            data.Add(new Snoop.Data.Double("Rounding", opt.Accuracy));
            data.Add(new Snoop.Data.Bool("Suppress trailing zeros", opt.SuppressTrailingZeros));
            data.Add(new Snoop.Data.Bool("Suppress leading zeros", opt.SuppressLeadingZeros));
            data.Add(new Snoop.Data.Bool("Suppress spaces", opt.SuppressSpaces));
            data.Add(new Snoop.Data.Bool("Use plus prefix", opt.UsePlusPrefix));
            data.Add(new Snoop.Data.Bool("Use digit grouping", opt.UseDigitGrouping));
         }
      }

      private void
      Stream(ArrayList data, CurtainGrid grid)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CurtainGrid)));

         data.Add(new Snoop.Data.Enumerable("Curtain Cells", grid.GetCurtainCells()));
         data.Add(new Snoop.Data.Enumerable("Mullions", grid.GetMullionIds(), m_activeDoc));
         data.Add(new Snoop.Data.Enumerable("Unlocked mullions", grid.GetUnlockedMullionIds(), m_activeDoc));
         data.Add(new Snoop.Data.Angle("Grid 1 angle", grid.Grid1Angle));
         data.Add(new Snoop.Data.String("Grid 1 justification", grid.Grid1Justification.ToString()));
         data.Add(new Snoop.Data.Double("Grid 1 offset", grid.Grid1Offset));
         data.Add(new Snoop.Data.Angle("Grid 2 angle", grid.Grid2Angle));
         data.Add(new Snoop.Data.String("Grid 2 justification", grid.Grid2Justification.ToString()));
         data.Add(new Snoop.Data.Double("Grid 2 offset", grid.Grid2Offset));
         data.Add(new Snoop.Data.Int("Number of panels", grid.NumPanels));
         data.Add(new Snoop.Data.Enumerable("Panels", grid.GetPanelIds(), m_activeDoc));
         data.Add(new Snoop.Data.Enumerable("Unlocked panels", grid.GetUnlockedPanelIds(), m_activeDoc));
         data.Add(new Snoop.Data.Int("Number of U lines", grid.NumULines));
         data.Add(new Snoop.Data.Enumerable("U grid lines", grid.GetUGridLineIds(), m_activeDoc));
         data.Add(new Snoop.Data.Int("Number of V lines", grid.NumVLines));
         data.Add(new Snoop.Data.Enumerable("V grid lines", grid.GetVGridLineIds(), m_activeDoc));
      }

      private void
      Stream(ArrayList data, CurtainCell cell)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CurtainCell)));

         data.Add(new Snoop.Data.Enumerable("Curve loops", cell.CurveLoops));
         data.Add(new Snoop.Data.Enumerable("Planarized curve loops", cell.PlanarizedCurveLoops));
      }

      private void
      Stream(ArrayList data, RebarHostData rebarHostData)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarHostData)));

         data.Add(new Snoop.Data.Bool("IsValidHost", rebarHostData.IsValidHost()));
      }

      private void
      Stream(ArrayList data, Leader leader)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Leader)));

         data.Add(new Snoop.Data.Xyz("Elbow", leader.Elbow));
         data.Add(new Snoop.Data.Xyz("End", leader.End));
      }

      private void
      Stream(ArrayList data, AreaVolumeSettings areaSettings)
      {
          data.Add(new Snoop.Data.ClassSeparator(typeof(AreaVolumeSettings)));

          data.Add(new Snoop.Data.String("Room area boundary location", areaSettings.GetSpatialElementBoundaryLocation(SpatialElementType.Room).ToString()));
          data.Add(new Snoop.Data.Bool("Volume computation enable", areaSettings.ComputeVolumes));
      }

      private void
      Stream(ArrayList data, ViewSheetSetting viewSheetSetting)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ViewSheetSetting)));

         data.Add(new Snoop.Data.Object("Available views", viewSheetSetting.AvailableViews));
         data.Add(new Snoop.Data.Object("Current view sheet set", viewSheetSetting.CurrentViewSheetSet));
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.UI.Events.DialogBoxData dlgBoxData)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.UI.Events.DialogBoxData)));

         data.Add(new Snoop.Data.Int("Help id", dlgBoxData.HelpId));
      }

      private void
      Stream(ArrayList data, BuildingSiteExportOptions bldSiteOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(BuildingSiteExportOptions)));

         data.Add(new Snoop.Data.Double("Area per person", bldSiteOptions.AreaPerPerson));
         data.Add(new Snoop.Data.Object("Property line", bldSiteOptions.PropertyLine));
         data.Add(new Snoop.Data.Double("Property line offset", bldSiteOptions.PropertyLineOffset));
         data.Add(new Snoop.Data.Double("Total gross area", bldSiteOptions.TotalGrossArea));
         data.Add(new Snoop.Data.Int("Total occupancy", bldSiteOptions.TotalOccupancy));
      }

      private void
      Stream(ArrayList data, Construction construct)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Construction)));

         data.Add(new Snoop.Data.String("Id", construct.Id));
         data.Add(new Snoop.Data.String("Name", construct.Name));
      }

      private void
      Stream(ArrayList data, FamilyElementVisibility famElemVisib)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyElementVisibility)));

         data.Add(new Snoop.Data.Bool("Is shown in Coarse", famElemVisib.IsShownInCoarse));
         data.Add(new Snoop.Data.Bool("Is shown in Fine", famElemVisib.IsShownInFine));
         data.Add(new Snoop.Data.Bool("Is shown in FrontBack", famElemVisib.IsShownInFrontBack));
         data.Add(new Snoop.Data.Bool("Is shown in LeftRight", famElemVisib.IsShownInLeftRight));
         data.Add(new Snoop.Data.Bool("Is shown in Medium", famElemVisib.IsShownInMedium));
         data.Add(new Snoop.Data.Bool("Is shown in PlanRCPCut", famElemVisib.IsShownInPlanRCPCut));
         data.Add(new Snoop.Data.Bool("Is shown in TopBottom", famElemVisib.IsShownInTopBottom));
         data.Add(new Snoop.Data.Bool("Is shown only when cut", famElemVisib.IsShownOnlyWhenCut));
         data.Add(new Snoop.Data.String("Visibility type", famElemVisib.VisibilityType.ToString()));
      }

      private void
      Stream(ArrayList data, FamilyManager famManager)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyManager)));

         data.Add(new Snoop.Data.Object("Current type", famManager.CurrentType));
         data.Add(new Snoop.Data.Enumerable("Parameters", famManager.Parameters));
         data.Add(new Snoop.Data.Enumerable("Types", famManager.Types));
      }

      private void
      Stream(ArrayList data, FamilyParameter famParam)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyParameter)));

         data.Add(new Snoop.Data.Enumerable("Associated parameters", famParam.AssociatedParameters));
         data.Add(new Snoop.Data.Bool("Can assign formula", famParam.CanAssignFormula));
         data.Add(new Snoop.Data.Object("Definition", famParam.Definition));
         try
         {   // this only works for certain types of Parameters
            data.Add(new Snoop.Data.String("Display unit type", famParam.DisplayUnitType.ToString()));
         }
         catch (System.Exception)
         {
            data.Add(new Snoop.Data.String("Display unit type", "N/A"));
         }
         data.Add(new Snoop.Data.String("Formula", famParam.Formula));
         data.Add(new Snoop.Data.Bool("Is determined by formula", famParam.IsDeterminedByFormula));
         data.Add(new Snoop.Data.Bool("Is instance", famParam.IsInstance));
         data.Add(new Snoop.Data.String("Storage type", famParam.StorageType.ToString()));
      }

      private void
      Stream(ArrayList data, FamilyType famType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyType)));

         data.Add(new Snoop.Data.String("Name", famType.Name));
      }

      private void
      Stream(ArrayList data, MEPSpaceConstruction mepSpaceConstr)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MEPSpaceConstruction)));

         data.Add(new Snoop.Data.Object("Current construction", mepSpaceConstr.CurrentConstruction));
         data.Add(new Snoop.Data.Enumerable("Space constructions", mepSpaceConstr.SpaceConstructions));
      }

      private void
      Stream(ArrayList data, DGNExportOptions dgnExportOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DGNExportOptions)));

         data.Add(new Snoop.Data.String("Layer mapping", dgnExportOptions.LayerMapping));
         data.Add(new Snoop.Data.String("Seed file name", dgnExportOptions.SeedName));
      }

      private void
      Stream(ArrayList data, DWFExportOptions dwfExportOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DWFExportOptions)));

         data.Add(new Snoop.Data.Bool("Exporting areas", dwfExportOptions.ExportingAreas));
         data.Add(new Snoop.Data.Bool("Export object data", dwfExportOptions.ExportObjectData));
         data.Add(new Snoop.Data.Bool("Merged views", dwfExportOptions.MergedViews));
         data.Add(new Snoop.Data.Bool("Stop on error", dwfExportOptions.StopOnError));

         DWFExportOptions dwf2dExpOptions = dwfExportOptions as DWFExportOptions;
         if (dwf2dExpOptions != null)
         {
            Stream(data, dwf2dExpOptions);
            return;
         }

         DWFExportOptions dwf3dExpOptions = dwfExportOptions as DWFExportOptions;
         if (dwf3dExpOptions != null)
         {
            Stream(data, dwf3dExpOptions);
            return;
         }
      }

      private void Stream(ArrayList data, DWFXExportOptions dwf2dExpOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DWFExportOptions)));

         data.Add(new Snoop.Data.String("Image format", dwf2dExpOptions.ImageFormat.ToString()));
         data.Add(new Snoop.Data.String("Image quality", dwf2dExpOptions.ImageQuality.ToString()));
         data.Add(new Snoop.Data.String("Paper format", dwf2dExpOptions.PaperFormat.ToString()));
         data.Add(new Snoop.Data.Bool("Portrait layout", dwf2dExpOptions.PortraitLayout));

         DWFXExportOptions dwfx2dExpOptions = dwf2dExpOptions as DWFXExportOptions;
         if (dwfx2dExpOptions != null)
         {
            Stream(data, dwfx2dExpOptions);
            return;
         }
      }

      private void Stream(ArrayList data, DWGExportOptions dwgExpOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DWGExportOptions)));

         data.Add(new Snoop.Data.Bool("Exporting areas", dwgExpOptions.ExportingAreas));
         data.Add(new Snoop.Data.String("Export of solids", dwgExpOptions.ExportOfSolids.ToString()));
         data.Add(new Snoop.Data.String("File version", dwgExpOptions.FileVersion.ToString()));
         data.Add(new Snoop.Data.String("File version", dwgExpOptions.LayerMapping));
         data.Add(new Snoop.Data.String("Line scaling", dwgExpOptions.LineScaling.ToString()));
         data.Add(new Snoop.Data.String("Prop overrides", dwgExpOptions.PropOverrides.ToString()));
         data.Add(new Snoop.Data.Bool("Shared coords", dwgExpOptions.SharedCoords));
         data.Add(new Snoop.Data.String("Target unit", dwgExpOptions.TargetUnit.ToString()));
         data.Add(new Snoop.Data.Bool("Merged view", dwgExpOptions.MergedViews));
      }

      private void
      Stream(ArrayList data, DWGImportOptions dwgImpOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DWGImportOptions)));

         // No data at this level yet.
      }

      private void
      Stream(ArrayList data, FBXExportOptions fbxExpOptions)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FBXExportOptions)));

         data.Add(new Snoop.Data.Bool("Stop on error", fbxExpOptions.StopOnError));
      }

      private void
      Stream(ArrayList data, TrussMemberInfo trussInfo)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(TrussMemberInfo)));

         data.Add(new Snoop.Data.Int("Stop on error", trussInfo.hostTrussId.IntegerValue));
         data.Add(new Snoop.Data.Bool("Locked to truss", trussInfo.lockedToTruss));
         data.Add(new Snoop.Data.String("Member type key", trussInfo.memberTypeKey.ToString()));
      }

      private void
      Stream(ArrayList data, VertexIndexPair vertIndPair)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(VertexIndexPair)));

         data.Add(new Snoop.Data.Int("Bottom", vertIndPair.Bottom));
         data.Add(new Snoop.Data.Int("Top", vertIndPair.Top));
      }

      private void
      Stream(ArrayList data, PointElementReference ptElemRef)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointElementReference)));

         PointOnEdge ptOnEdge = ptElemRef as PointOnEdge;
         if (ptOnEdge != null)
         {
            Stream(data, ptOnEdge);
            return;
         }

         PointOnEdgeEdgeIntersection ptOnEdgeEdgeInt = ptElemRef as PointOnEdgeEdgeIntersection;
         if (ptOnEdgeEdgeInt != null)
         {
            Stream(data, ptOnEdgeEdgeInt);
            return;
         }

         PointOnEdgeFaceIntersection ptOnEdgeFaceInt = ptElemRef as PointOnEdgeFaceIntersection;
         if (ptOnEdgeFaceInt != null)
         {
            Stream(data, ptOnEdgeFaceInt);
            return;
         }

         PointOnFace ptOnFace = ptElemRef as PointOnFace;
         if (ptOnFace != null)
         {
            Stream(data, ptOnFace);
            return;
         }

         //PointOnSketch ptOnSketch = ptElemRef as PointOnSketch;
         //if (ptOnSketch != null) {
         //    Stream(data, ptOnSketch);
         //    return;
         //}

         //PointRelativeToPoint ptRelToPt = ptElemRef as PointRelativeToPoint;
         //if (ptRelToPt != null) {
         //    Stream(data, ptRelToPt);
         //    return;
         //}
      }

      private void
      Stream(ArrayList data, PointOnEdge ptOnEdge)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointOnEdge)));
         data.Add(new Snoop.Data.Object("PointLocationOnCurve", ptOnEdge.LocationOnCurve));
         data.Add(new Snoop.Data.Object("Reference", ptOnEdge.GetEdgeReference()));
      }

      private void
      Stream(ArrayList data, PointLocationOnCurve ptLocOnCurve)
      {
         data.Add(new Snoop.Data.String("Measurement Type", ptLocOnCurve.MeasurementType.ToString()));
         data.Add(new Snoop.Data.String("Measure From", ptLocOnCurve.MeasureFrom.ToString()));
         data.Add(new Snoop.Data.Double("Measurement Value", ptLocOnCurve.MeasurementValue));
      }

      private void
      Stream(ArrayList data, PointOnEdgeEdgeIntersection ptOnEdgeEdgeInt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointOnEdgeEdgeIntersection)));

         data.Add(new Snoop.Data.Object("Edge 1", ptOnEdgeEdgeInt.GetEdgeReference1()));
         data.Add(new Snoop.Data.Object("Edge 2", ptOnEdgeEdgeInt.GetEdgeReference2()));
      }

      private void
      Stream(ArrayList data, PointOnEdgeFaceIntersection ptOnEdgeFaceInt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointOnEdgeFaceIntersection)));

         data.Add(new Snoop.Data.Object("Edge", ptOnEdgeFaceInt.GetEdgeReference()));
         data.Add(new Snoop.Data.Object("Face", ptOnEdgeFaceInt.GetFaceReference()));
         data.Add(new Snoop.Data.Bool("Orient with edge", ptOnEdgeFaceInt.OrientWithEdge));
      }

      private void
      Stream(ArrayList data, PointOnFace ptOnFace)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointOnFace)));

         data.Add(new Snoop.Data.Object("Reference", ptOnFace.GetFaceReference()));
         data.Add(new Snoop.Data.Uv("UV", ptOnFace.UV));
      }

      //private void
      //Stream(ArrayList data, PointOn ptOnSketch)
      //{
      //    data.Add(new Snoop.Data.ClassSeparator(typeof(PointOnSketch)));

      //    data.Add(new Snoop.Data.Double("Offset", ptOnSketch.Offset));
      //    data.Add(new Snoop.Data.Object("Sketch", ptOnSketch.Sketch));
      //    data.Add(new Snoop.Data.Uv("UV", ptOnSketch.UV));
      //}

      //private void
      //Stream(ArrayList data, Point PointRelativeToPoint ptRelToPt)
      //{
      //    data.Add(new Snoop.Data.ClassSeparator(typeof(PointRelativeToPoint)));

      //    data.Add(new Snoop.Data.Object("Base point", ptRelToPt.GetBasePoint() ));
      //    data.Add(new Snoop.Data.Object("Transform", ptRelToPt.GetTransform() ));            
      //}

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Architecture.BoundarySegment boundSeg)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Architecture.BoundarySegment)));

         data.Add(new Snoop.Data.Object("Curve", boundSeg.Curve));
         data.Add(new Snoop.Data.Object("Document", boundSeg.Document));
         data.Add(new Snoop.Data.Object("Element", boundSeg.Element));
      }

      //TF
      private void
      Stream(ArrayList data, RebarConstraintsManager rbcm)
      {
          data.Add(new Snoop.Data.ClassSeparator(typeof(RebarConstraintsManager)));

          data.Add(new Snoop.Data.Enumerable("ConstrainedHandles", rbcm.GetAllConstrainedHandles()));
          foreach (RebarConstrainedHandle ch in rbcm.GetAllConstrainedHandles())
          {
              data.Add(new Snoop.Data.Object("Current Constraint on Handle", rbcm.GetCurrentConstraintOnHandle(ch)));
              data.Add(new Snoop.Data.Object("User Preferred Constraint on Handle", rbcm.GetPreferredConstraintOnHandle(ch)));
              data.Add(new Snoop.Data.Enumerable("ConstraintCandidatesFor Handle", rbcm.GetConstraintCandidatesForHandle(ch)));
          }

      }

      private void
      Stream(ArrayList data, RebarConstrainedHandle rbch)
      {
          data.Add(new Snoop.Data.ClassSeparator(typeof(RebarConstrainedHandle)));

          if (rbch.IsEdgeHandle()) data.Add(new Snoop.Data.Int("EdgeNumber", rbch.GetEdgeNumber()));
          data.Add(new Snoop.Data.Bool("IsValid", rbch.IsValid()));
          data.Add(new Snoop.Data.String("HandleType", rbch.GetHandleType().ToString()));
          if (rbch.IsEdgeHandle()) data.Add(new Snoop.Data.Int("Edge Number", rbch.GetEdgeNumber()));
      }

      private void
      Stream(ArrayList data, RebarConstraint rbc)
      {
          data.Add(new Snoop.Data.ClassSeparator(typeof(RebarConstraint)));

          data.Add(new Snoop.Data.Bool("IsValid", rbc.IsValid()));
          data.Add(new Snoop.Data.String("ConstraintType", rbc.GetConstraintType().ToString()));
          if (rbc.GetConstraintType() == RebarConstraintType.FixedDistanceToHostFace)
          {
              data.Add(new Snoop.Data.Double("Distance to Target Host Face", rbc.GetDistanceToTargetHostFace()));
          }
          if (rbc.GetConstraintType() != RebarConstraintType.ToOtherRebar)
          {
              data.Add(new Snoop.Data.String("Target Host Face Type", rbc.GetRebarConstraintTargetHostFaceType().ToString()));
          }
          if (rbc.GetConstraintType() == RebarConstraintType.ToOtherRebar)
          {
              data.Add(new Snoop.Data.String("Rebar Constraint Type", rbc.GetTargetRebarConstraintType().ToString()));
              if (rbc.GetTargetRebarConstraintType() == TargetRebarConstraintType.BarBend)
              {
                  data.Add(new Snoop.Data.Int("Target RebarBendNumber", rbc.GetTargetRebarBendNumber()));
              }
          }
      }
       
      //TFEND   
   }
}
