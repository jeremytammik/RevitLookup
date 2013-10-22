#region Header
//
// Copyright 2003-2013 by Autodesk, Inc. 
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

using Autodesk.Revit;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Architecture;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
   /// <summary>
   /// Provide Snoop.Data for any classes related to an Symbols.
   /// </summary>

   public class CollectorExtSymbol : CollectorExt
   {
      public
      CollectorExtSymbol()
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
         ElementType sym = e.ObjToSnoop as ElementType;
         if (sym != null)
         {
            Stream(snoopCollector.Data(), sym);
            return;
         }


         RebarShapeDefinition rebarShapeDef = e.ObjToSnoop as RebarShapeDefinition;
         if (rebarShapeDef != null)
         {
            Stream(snoopCollector.Data(), rebarShapeDef);
            return;
         }
      }


      private void
      Stream(ArrayList data, ElementType sym)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ElementType)));

         // no data at this level yet

         AnnotationSymbolType annoType = sym as AnnotationSymbolType;
         if (annoType != null)
         {
            Stream(data, annoType);
            return;
         }

         AreaReinforcementType areaReinforcementType = sym as AreaReinforcementType;
         if (areaReinforcementType != null)
         {
            Stream(data, areaReinforcementType);
            return;
         }

         AreaTagType areaTagType = sym as AreaTagType;
         if (areaTagType != null)
         {
            Stream(data, areaTagType);
            return;
         }

         BeamSystemType beamSystemType = sym as BeamSystemType;
         if (beamSystemType != null)
         {
            Stream(data, beamSystemType);
            return;
         }

         DimensionType dimType = sym as DimensionType;
         if (dimType != null)
         {
            Stream(data, dimType);
            return;
         }

         //TF
         FabricSheetType fabricST = sym as FabricSheetType;
         if (fabricST != null)
         {
            Stream(data, fabricST);
            return;
         }

         FabricWireType fabricWT = sym as FabricWireType;
         if (fabricWT != null)
         {
            Stream(data, fabricWT);
            return;
         }
         //TFEND

         GroupType groupType = sym as GroupType;
         if (groupType != null)
         {
            Stream(data, groupType);
            return;
         }

         HostObjAttributes hostAtt = sym as HostObjAttributes;
         if (hostAtt != null)
         {
            Stream(data, hostAtt);
            return;
         }

         InsertableObject insObj = sym as InsertableObject;
         if (insObj != null)
         {
            Stream(data, insObj);
            return;
         }

         LevelType levelType = sym as LevelType;
         if (levelType != null)
         {
            Stream(data, levelType);
            return;
         }

         LineAndTextAttrSymbol lineAndTextAttr = sym as LineAndTextAttrSymbol;
         if (lineAndTextAttr != null)
         {
            Stream(data, lineAndTextAttr);
            return;
         }

         LoadTypeBase loadTypeBase = sym as LoadTypeBase;
         if (loadTypeBase != null)
         {
            Stream(data, loadTypeBase);
            return;
         }

         MEPBuildingConstruction mepBldConst = sym as MEPBuildingConstruction;
         if (mepBldConst != null)
         {
            Stream(data, mepBldConst);
            return;
         }

         PathReinforcementType pathReinforcementType = sym as PathReinforcementType;
         if (pathReinforcementType != null)
         {
            Stream(data, pathReinforcementType);
            return;
         }

         RebarBarType rebarBarType = sym as RebarBarType;
         if (rebarBarType != null)
         {
            Stream(data, rebarBarType);
            return;
         }

         RebarCoverType rebarCoverType = sym as RebarCoverType;
         if (rebarCoverType != null)
         {
            Stream(data, rebarCoverType);
            return;
         }

         RebarHookType rebarHookType = sym as RebarHookType;
         if (rebarHookType != null)
         {
            Stream(data, rebarHookType);
            return;
         }

         RebarShape rebarShape = sym as RebarShape;
         if (rebarShape != null)
         {
            Stream(data, rebarShape);
            return;
         }

         RoomTagType roomTagType = sym as RoomTagType;
         if (roomTagType != null)
         {
            Stream(data, roomTagType);
            return;
         }

         SpaceTagType spaceTagType = sym as SpaceTagType;
         if (spaceTagType != null)
         {
            Stream(data, spaceTagType);
            return;
         }

         TrussType trussType = sym as TrussType;
         if (trussType != null)
         {
            Stream(data, trussType);
            return;
         }
         
         DistributionSysType distSysType = sym as DistributionSysType;
         if (distSysType != null)
         {
            Stream(data, distSysType);
            return;
         }

         MEPCurveType mepCurType = sym as MEPCurveType;
         if (mepCurType != null)
         {
            Stream(data, mepCurType);
            return;
         }

         FluidType fluidType = sym as FluidType;
         if (fluidType != null)
         {
            Stream(data, fluidType);
            return;
         }

         PipeScheduleType pipeSchedType = sym as PipeScheduleType;
         if (pipeSchedType != null)
         {
            Stream(data, pipeSchedType);
            return;
         }

         VoltageType voltType = sym as VoltageType;
         if (voltType != null)
         {
            Stream(data, voltType);
            return;
         }

         WireType wireType = sym as WireType;
         if (wireType != null)
         {
            Stream(data, wireType);
            return;
         }

         ModelTextType modelTxtType = sym as ModelTextType;
         if (modelTxtType != null)
         {
            Stream(data, modelTxtType);
            return;
         }
      }

      private void
      Stream(ArrayList data, HostObjAttributes hostAtt)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(HostObjAttributes)));

         data.Add(new Snoop.Data.Object("Compound Structure", hostAtt.GetCompoundStructure()));

         ContFootingType contFootingType = hostAtt as ContFootingType;
         if (contFootingType != null)
         {
            Stream(data, contFootingType);
            return;
         }

         CurtainSystemType curSysType = hostAtt as CurtainSystemType;
         if (curSysType != null)
         {
            Stream(data, curSysType);
            return;
         }

         FloorType floorType = hostAtt as FloorType;
         if (floorType != null)
         {
            Stream(data, floorType);
            return;
         }

         HostedSweepType hostSweepType = hostAtt as HostedSweepType;
         if (hostSweepType != null)
         {
            Stream(data, hostSweepType);
            return;
         }

         RoofType roofType = hostAtt as RoofType;
         if (roofType != null)
         {
            Stream(data, roofType);
            return;
         }

         WallType wallType = hostAtt as WallType;
         if (wallType != null)
         {
            Stream(data, wallType);
            return;
         }
      }

      private void
      Stream(ArrayList data, ContFootingType contFootingType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ContFootingType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, CurtainSystemType curSysType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CurtainSystemType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, FloorType floorType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FloorType)));

         data.Add(new Snoop.Data.Bool("Is foundation slab", floorType.IsFoundationSlab));
      }

      private void
      Stream(ArrayList data, HostedSweepType hostSweepType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(HostedSweepType)));

         // No data at this level yet!

         FasciaType fasciaType = hostSweepType as FasciaType;
         if (fasciaType != null)
         {
            Stream(data, fasciaType);
            return;
         }

         Autodesk.Revit.DB.Architecture.GutterType gutterType = hostSweepType as Autodesk.Revit.DB.Architecture.GutterType;
         if (gutterType != null)
         {
            Stream(data, gutterType);
            return;
         }

         SlabEdgeType slabEdgeType = hostSweepType as SlabEdgeType;
         if (slabEdgeType != null)
         {
            Stream(data, slabEdgeType);
            return;
         }
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Architecture.FasciaType fasciaType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Architecture.FasciaType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Architecture.GutterType gutterType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Architecture.GutterType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, SlabEdgeType gutterType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(SlabEdgeType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, RoofType roofType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RoofType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, WallType wallType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(WallType)));

         data.Add(new Snoop.Data.Double("Width", wallType.Width));
         data.Add(new Snoop.Data.String("Kind", wallType.Kind.ToString()));
      }

      private void
      Stream(ArrayList data, InsertableObject insObj)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(InsertableObject)));

         // no data at this level yet!

         FamilySymbol famSym = insObj as FamilySymbol;
         if (famSym != null)
         {
            Stream(data, famSym);
            return;
         }
      }

      private void
      Stream(ArrayList data, FamilySymbol famSym)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FamilySymbol)));

         data.Add(new Snoop.Data.Object("Family", famSym.Family));
         data.Add(new Snoop.Data.Enumerable("Material", famSym.GetMaterialIds(false)));

         MullionType mullionType = famSym as MullionType;
         if (mullionType != null)
         {
            Stream(data, mullionType);
            return;
         }

         PanelType panelType = famSym as PanelType;
         if (panelType != null)
         {
            Stream(data, panelType);
            return;
         }
      }

      private void
      Stream(ArrayList data, MullionType mullionType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MullionType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, PanelType panelType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PanelType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, GroupType groupType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(GroupType)));

         data.Add(new Snoop.Data.Enumerable("Groups", groupType.Groups));
      }

      private void
      Stream(ArrayList data, AreaReinforcementType areaReinforcementType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AreaReinforcementType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, AreaTagType areaTagType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AreaTagType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, BeamSystemType beamSystemType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(BeamSystemType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, LevelType levelType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LevelType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, PathReinforcementType pathReinforcementType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PathReinforcementType)));

         // No data at this level yet!

      }

      private void
      Stream(ArrayList data, LineAndTextAttrSymbol lineAttr)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LineAndTextAttrSymbol)));

         GridType gridType = lineAttr as GridType;
         if (gridType != null)
         {
            Stream(data, gridType);
            return;
         }

         TextElementType textAttr = lineAttr as TextElementType;
         if (textAttr != null)
         {
            Stream(data, textAttr);
            return;
         }

      }

      private void
      Stream(ArrayList data, GridType gridType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(GridType)));

         // no data at this level
      }

      private void
      Stream(ArrayList data, TextElementType textAttr)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(TextElementType)));

         // no data at this level

         TextNoteType textNoteAttr = textAttr as TextNoteType;
         if (textNoteAttr != null)
         {
            Stream(data, textNoteAttr);
            return;
         }
      }

      private void
      Stream(ArrayList data, TextNoteType textAttr)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(TextNoteType)));

         // no data at this level
      }

      private void
      Stream(ArrayList data, AnnotationSymbolType annoType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnnotationSymbolType)));

         // no data at this level
      }

      private void
      Stream(ArrayList data, DimensionType dimType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DimensionType)));

         // No data at this level yet!

         SpotDimensionType spotDimType = dimType as SpotDimensionType;
         if (spotDimType != null)
         {
            Stream(data, spotDimType);
            return;
         }
      }

      private void
      Stream(ArrayList data, SpotDimensionType spotDimType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(SpotDimensionType)));

         // No data at this level yet!
      }

      private void
      Stream(ArrayList data, RebarBarType rebarType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarBarType)));

         // TBD: how to call functions
      }

      private void
      Stream(ArrayList data, RebarCoverType rebarCoverType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarCoverType)));

         data.Add(new Snoop.Data.Double("Cover distance", rebarCoverType.CoverDistance));
      }

      private void
      Stream(ArrayList data, RebarHookType rebarHookType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarHookType)));

         // Nothing at this level yet!
      }

      private void
      Stream(ArrayList data, RebarShape rebarShape)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarShape)));

         data.Add(new Snoop.Data.Object("GetRebarShapeDefinition()", rebarShape.GetRebarShapeDefinition()));
         data.Add(new Snoop.Data.String("Rebar style", rebarShape.RebarStyle.ToString()));
         data.Add(new Snoop.Data.Bool("Simple arc", rebarShape.SimpleArc));      // TBD: should be "IsSimpleArc"?
         data.Add(new Snoop.Data.Bool("Simple line", rebarShape.SimpleLine));    // TBD: should be "IsSimpleLine?"

         // TBD: how to call other functions?
      }

//TF
      private void Stream(ArrayList data, FabricSheetType fabricST)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FabricSheetType)));

         data.Add(new Snoop.Data.ElementId("Material", fabricST.Material, m_app.ActiveUIDocument.Document));
         data.Add(new Snoop.Data.Double("Overall Length", fabricST.OverallLength));
         data.Add(new Snoop.Data.Double("Overall Width", fabricST.OverallWidth));
         data.Add(new Snoop.Data.Double("Sheet Mass", fabricST.SheetMass));

         data.Add(new Snoop.Data.CategorySeparator("Major Direction"));
         data.Add(new Snoop.Data.ElementId("Wire Type", fabricST.MajorDirectionWireType, m_app.ActiveUIDocument.Document));
         data.Add(new Snoop.Data.Double("End Overhang", fabricST.MajorEndOverhang));
         data.Add(new Snoop.Data.Double("Lap Splice Length", fabricST.MajorLapSpliceLength));
         data.Add(new Snoop.Data.String("Layout Pattern", fabricST.MajorLayoutPattern.ToString()));
         data.Add(new Snoop.Data.Double("Number of Wires", fabricST.MajorNumberOfWires));
         data.Add(new Snoop.Data.Double("Reinforcement Area", fabricST.MajorReinforcementArea));
         data.Add(new Snoop.Data.Double("Spacing", fabricST.MajorSpacing));
         data.Add(new Snoop.Data.Double("Start Overhang", fabricST.MajorStartOverhang));
         data.Add(new Snoop.Data.CategorySeparator("Minor Direction"));
         data.Add(new Snoop.Data.ElementId("Wire Type", fabricST.MinorDirectionWireType, m_app.ActiveUIDocument.Document));
         data.Add(new Snoop.Data.Double("End Overhang", fabricST.MinorEndOverhang));
         data.Add(new Snoop.Data.Double("Lap Splice Length", fabricST.MinorLapSpliceLength));
         data.Add(new Snoop.Data.String("Layout Pattern", fabricST.MinorLayoutPattern.ToString()));
         data.Add(new Snoop.Data.Double("Number of Wires", fabricST.MinorNumberOfWires));
         data.Add(new Snoop.Data.Double("Reinforcement Area", fabricST.MinorReinforcementArea));
         data.Add(new Snoop.Data.Double("Spacing", fabricST.MinorSpacing));
         data.Add(new Snoop.Data.Double("Start Overhang", fabricST.MinorStartOverhang));
         
      }

      private void Stream(ArrayList data, FabricWireType fabricWT)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FabricWireType)));

         data.Add(new Snoop.Data.Double("Wire Diameter", fabricWT.WireDiameter));
      }      
//TFEND

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Architecture.RoomTagType roomTagType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Architecture.RoomTagType)));

         // Nothing at this level yet!
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Mechanical.SpaceTagType spaceTagType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Mechanical.SpaceTagType)));

         // Nothing at this level yet!
      }

      private void
      Stream(ArrayList data, TrussType trussType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(TrussType)));

         // Nothing at this level yet!
      }

      private void
      Stream(ArrayList data, Autodesk.Revit.DB.Electrical.ElectricalDemandFactorDefinition demandFactorDefinition)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.Electrical.ElectricalDemandFactorDefinition)));

         data.Add(new Snoop.Data.String("AdditionalLoad", demandFactorDefinition.AdditionalLoad.ToString()));
         data.Add(new Snoop.Data.Bool("IncludeAdditionalLoad", demandFactorDefinition.IncludeAdditionalLoad));
         data.Add(new Snoop.Data.String("RuleType", demandFactorDefinition.RuleType.ToString()));
      }

      private void
      Stream(ArrayList data, DistributionSysType distSysType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DistributionSysType)));

         data.Add(new Snoop.Data.String("Electrical phase", distSysType.ElectricalPhase.ToString()));
         data.Add(new Snoop.Data.String("Electrical phase config", distSysType.ElectricalPhaseConfiguration.ToString()));
         data.Add(new Snoop.Data.Bool("Is in use", distSysType.IsInUse));
         data.Add(new Snoop.Data.Int("Num wires", distSysType.NumWires));
         data.Add(new Snoop.Data.Object("Voltage line to ground", distSysType.VoltageLineToGround));
         data.Add(new Snoop.Data.Object("Voltage line to line", distSysType.VoltageLineToLine));
      }

      private void
      Stream(ArrayList data, MEPCurveType mepCurType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MEPCurveType)));

         data.Add(new Snoop.Data.Object("Cross", mepCurType.Cross));
         data.Add(new Snoop.Data.Object("Elbow", mepCurType.Elbow));
         data.Add(new Snoop.Data.Object("Multi shape transition", mepCurType.MultiShapeTransition));
         data.Add(new Snoop.Data.String("Preferred junction type", mepCurType.PreferredJunctionType.ToString()));
         data.Add(new Snoop.Data.Double("Roughness", mepCurType.Roughness));
         data.Add(new Snoop.Data.Object("Tap", mepCurType.Tap));
         data.Add(new Snoop.Data.Object("Tee", mepCurType.Tee));
         data.Add(new Snoop.Data.Object("Transition", mepCurType.Transition));
         data.Add(new Snoop.Data.Object("Union", mepCurType.Union));

         DuctType ductType = mepCurType as DuctType;
         if (ductType != null)
         {
            Stream(data, ductType);
            return;
         }

         FlexDuctType flexDuctType = mepCurType as FlexDuctType;
         if (flexDuctType != null)
         {
            Stream(data, flexDuctType);
            return;
         }

         PipeType pipeType = mepCurType as PipeType;
         if (pipeType != null)
         {
            Stream(data, pipeType);
            return;
         }

         FlexPipeType flexPipeType = mepCurType as FlexPipeType;
         if (flexPipeType != null)
         {
            Stream(data, flexPipeType);
            return;
         }
      }

      private void
      Stream(ArrayList data, DuctType ductType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(DuctType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, FlexDuctType flexDuctType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FlexDuctType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, PipeType pipeType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PipeType)));

         data.Add(new Snoop.Data.Object("Class", pipeType.Class));
      }

      private void
      Stream(ArrayList data, FlexPipeType flexPipeType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FlexPipeType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, FluidType fluidType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(FluidType)));

         // Nothing at this level yet
      }


      private void
      Stream(ArrayList data, PipeScheduleType pipeSchedType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PipeScheduleType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, VoltageType voltType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(VoltageType)));

         data.Add(new Snoop.Data.Double("Actual value", voltType.ActualValue));
         data.Add(new Snoop.Data.Bool("Is in use", voltType.IsInUse));
         data.Add(new Snoop.Data.Double("Max value", voltType.MaxValue));
         data.Add(new Snoop.Data.Double("Min value", voltType.MinValue));
      }

      private void
      Stream(ArrayList data, WireType wireType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(WireType)));

         data.Add(new Snoop.Data.Object("Actual value", wireType.Conduit));
         data.Add(new Snoop.Data.Object("Is in use", wireType.Insulation));
         data.Add(new Snoop.Data.Bool("Is in use", wireType.IsInUse));
         data.Add(new Snoop.Data.Object("Max size", wireType.MaxSize));
         data.Add(new Snoop.Data.Double("Neutral multiplier", wireType.NeutralMultiplier));
         data.Add(new Snoop.Data.Bool("Neutral required", wireType.NeutralRequired));
         data.Add(new Snoop.Data.String("Neutral size", wireType.NeutralSize.ToString()));
         data.Add(new Snoop.Data.Object("Temperature rating", wireType.TemperatureRating));
         data.Add(new Snoop.Data.Object("Wire material", wireType.WireMaterial));
      }

      private void
      Stream(ArrayList data, ModelTextType modelTextType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(WireType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, LoadTypeBase loadBase)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LoadTypeBase)));

         // Nothing at this level yet

         AreaLoadType areaLoadType = loadBase as AreaLoadType;
         if (areaLoadType != null)
         {
            Stream(data, areaLoadType);
            return;
         }

         LineLoadType lineLoadType = loadBase as LineLoadType;
         if (lineLoadType != null)
         {
            Stream(data, lineLoadType);
            return;
         }

         PointLoadType pointLoadType = loadBase as PointLoadType;
         if (pointLoadType != null)
         {
            Stream(data, pointLoadType);
            return;
         }
      }

      private void
      Stream(ArrayList data, AreaLoadType areaLoadType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AreaLoadType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, LineLoadType lineLoadType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LineLoadType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, PointLoadType pointLoadType)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(PointLoadType)));

         // Nothing at this level yet
      }

      private void
      Stream(ArrayList data, MEPBuildingConstruction mepBldgConst)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MEPBuildingConstruction)));

         // TBD: how to call these functions?
      }

      private void
      Stream(ArrayList data, RebarShapeDefinition rebarShapeDef)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarShapeDefinition)));

         data.Add(new Snoop.Data.Bool("Complete", rebarShapeDef.Complete));    // TBD: should be "IsComplete?"

         RebarShapeDefinitionByArc rebarShapeDefByArc = rebarShapeDef as RebarShapeDefinitionByArc;
         if (rebarShapeDefByArc != null)
         {
            Stream(data, rebarShapeDefByArc);
            return;
         }

         RebarShapeDefinitionBySegments rebarShapeDefBySegs = rebarShapeDef as RebarShapeDefinitionBySegments;
         if (rebarShapeDefBySegs != null)
         {
            Stream(data, rebarShapeDefBySegs);
            return;
         }
      }

      private void
      Stream(ArrayList data, RebarShapeDefinitionByArc rebarShapeDefByArc)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarShapeDefinitionByArc)));

         data.Add(new Snoop.Data.Object("Type", rebarShapeDefByArc.Type));
      }

      private void
      Stream(ArrayList data, RebarShapeDefinitionBySegments rebarShapeDefBySegs)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(RebarShapeDefinitionBySegments)));

         data.Add(new Snoop.Data.Int("Number of segments", rebarShapeDefBySegs.NumberOfSegments));
      }

   }
}
