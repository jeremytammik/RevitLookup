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

         StructuralAsset structuralAsset = e.ObjToSnoop as StructuralAsset;
         if (structuralAsset != null)
         {
            Stream(snoopCollector.Data(), structuralAsset);
            return;
         }
/* TF
         AnalyticalModelSweptProfile profile = e.ObjToSnoop as AnalyticalModelSweptProfile;
         if (profile != null)
         {
            Stream(snoopCollector.Data(), profile);
            return;
         }
*/
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

         data.Add(new Snoop.Data.Bool("CanApproximate", aModel.CanApproximate()));
         //data.Add(new Snoop.Data.Bool("CanDisable", aModel.CanDisable()));
         //data.Add(new Snoop.Data.Bool("CanDisableAutoDetect", aModel.CanDisableAutoDetect()));
         data.Add(new Snoop.Data.Bool("CanHaveRigidLinks", aModel.CanHaveRigidLinks()));
         //TF        data.Add(new Snoop.Data.Bool("CanSetAnalyticalOffset", aModel.CanSetAnalyticalOffset()));
         data.Add(new Snoop.Data.Bool("CanUseHardPoints", aModel.CanUseHardPoints()));
         data.Add(new Snoop.Data.Enumerable("Analytical Model Supports", aModel.GetAnalyticalModelSupports()));
         data.Add(new Snoop.Data.Object("Analyze As", aModel.GetAnalyzeAs()));

         try
         {
            data.Add(new Snoop.Data.Xyz("Offset EndOrTop", aModel.GetOffset(AnalyticalElementSelector.EndOrTop)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Offset EndOrTop", ex));
         }

         try
         {
            data.Add(new Snoop.Data.Xyz("Offset StartOrBase", aModel.GetOffset(AnalyticalElementSelector.StartOrBase)));

         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Offset StartOrBase", ex));
         }

         try
         {
            data.Add(new Snoop.Data.Xyz("Offset Whole", aModel.GetOffset(AnalyticalElementSelector.Whole)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Offset Whole", ex));
         }
/* TF
         if (aModel.HasSweptProfile())
            data.Add(new Snoop.Data.Object("Swept profile", aModel.GetSweptProfile()));
         else
            data.Add(new Snoop.Data.String("Swept profile", "No swept profile."));
*/
         if (aModel is AnalyticalModelSurface)
            Stream(data, (AnalyticalModelSurface)aModel);
         if (aModel is AnalyticalModelStick)
            Stream(data, (AnalyticalModelStick)aModel);
      }

      private void
      Stream(ArrayList data, AnalyticalModelSurface aModel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModelSurface)));
         try {
            data.Add(new Snoop.Data.ElementId("BottomExtensionPlaneId", aModel.BottomExtensionPlaneId, aModel.Document));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("BottomExtensionPlaneId", ex));
         }
         try {
          data.Add(new Snoop.Data.ElementId("TopExtensionPlaneId", aModel.TopExtensionPlaneId, aModel.Document));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("TopExtensionPlaneId", ex));
         }
         try {
          data.Add(new Snoop.Data.String("ProjectionZ", aModel.ProjectionZ.ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("ProjectionZ", ex));
         }
         try {
          data.Add(new Snoop.Data.String("BottomExtension", aModel.BottomExtension.ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("BottomExtension", ex));
         }try {
          data.Add(new Snoop.Data.String("TopExtension", aModel.TopExtension.ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("TopExtension", ex));
         }
         try {
          data.Add(new Snoop.Data.String("AlignmentMethod", aModel.AlignmentMethod.ToString()));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("AlignmentMethod", ex));
         }
         //
         data.Add(new Snoop.Data.Enumerable("Outer Loops", aModel.GetLoops(AnalyticalLoopType.External)));

         data.Add(new Snoop.Data.Enumerable("Inner Loops", aModel.GetLoops(AnalyticalLoopType.Internal)));

         System.Collections.Generic.ICollection<ElementId> openingIds = new System.Collections.Generic.List<ElementId>();
         aModel.GetOpenings(out openingIds);
         data.Add(new Snoop.Data.Enumerable("Openings", openingIds));

         System.Collections.Generic.ICollection<ElementId> hiddenOpeningIds = new System.Collections.Generic.List<ElementId>();
         aModel.GetHiddenOpenings(out hiddenOpeningIds);
         data.Add(new Snoop.Data.Enumerable("Hidden Openings", hiddenOpeningIds));
      }

      private void
     Stream(ArrayList data, AnalyticalModelStick aModel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModelStick)));

         try {
         data.Add(new Snoop.Data.String("GetAlignmentMethod-EndOrTop", aModel.GetAlignmentMethod(AnalyticalElementSelector.EndOrTop).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetAlignmentMethod-EndOrTop", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetAlignmentMethod-StartOrBase", aModel.GetAlignmentMethod(AnalyticalElementSelector.StartOrBase).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetAlignmentMethod-StartOrBase", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetAlignmentMethod-Whole ", aModel.GetAlignmentMethod(AnalyticalElementSelector.Whole).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetAlignmentMethod-Whole", ex));
         }
         //
         try {
         data.Add(new Snoop.Data.String("GetProjectionY-EndOrTop", aModel.GetProjectionY(AnalyticalElementSelector.EndOrTop).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionY-EndOrTop", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetProjectionY-StartOrBase", aModel.GetProjectionY(AnalyticalElementSelector.StartOrBase).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionY-StartOrBase", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetProjectionY-Whole ", aModel.GetProjectionY(AnalyticalElementSelector.Whole).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionY-Whole", ex));
         }
         //
         try {
         data.Add(new Snoop.Data.String("GetProjectionZ-EndOrTop", aModel.GetProjectionZ(AnalyticalElementSelector.EndOrTop).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionZ-EndOrTop", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetProjectionZ-StartOrBase", aModel.GetProjectionZ(AnalyticalElementSelector.StartOrBase).ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionZ-StartOrBase", ex));
         }
         try {
         data.Add(new Snoop.Data.String("GetProjectionZ-Whole ", aModel.GetProjectionZ(AnalyticalElementSelector.Whole).ToString()));
             }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GetProjectionZ-Whole", ex));
         }
         
         if (aModel is AnalyticalModelColumn)
            Stream(data, (AnalyticalModelColumn)aModel);
      }

      private void
      Stream(ArrayList data, AnalyticalModelColumn aModel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModelColumn)));

         try {
         data.Add(new Snoop.Data.String("BaseExtensionMethod", aModel.BaseExtensionMethod.ToString()));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("BaseExtensionMethod", ex));
         }
         try {
         data.Add(new Snoop.Data.String("TopExtensionMethod", aModel.TopExtensionMethod.ToString()));
             }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("TopExtensionMethod", ex));
         }
         try {
         data.Add(new Snoop.Data.ElementId("BaseExtensionPlaneId", aModel.BaseExtensionPlaneId, aModel.Document));
          }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("BaseExtensionPlaneId", ex));
         }
         try {
         data.Add(new Snoop.Data.ElementId("TopExtensionPlaneId", aModel.TopExtensionPlaneId, aModel.Document));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("TopExtensionPlaneId", ex));
         }
      }

      private void
      Stream(ArrayList data, CompoundStructure compStruct)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CompoundStructure)));

         data.Add(new Snoop.Data.Enumerable("Layers", compStruct.GetLayers()));

         int structuralMaterialIdx = compStruct.StructuralMaterialIndex;
         data.Add(new Snoop.Data.Int("Structural Material Index", structuralMaterialIdx));

         if (structuralMaterialIdx >= 0)
            data.Add(new Snoop.Data.ElementId("Structural Material (using StructuralMaterialIndex)", compStruct.GetMaterialId(structuralMaterialIdx), m_activeDoc));
         else
            data.Add(new Snoop.Data.ElementId("Structural Material (using StructuralMaterialIndex)", ElementId.InvalidElementId, m_activeDoc));

         data.Add(new Snoop.Data.Double("Width", compStruct.GetWidth()));
         data.Add(new Snoop.Data.Int("Variable Layer Index", compStruct.VariableLayerIndex));
         data.Add(new Snoop.Data.Bool("Is vertically compound", compStruct.IsVerticallyCompound));
         data.Add(new Snoop.Data.Bool("Is vertically homogeneous", compStruct.IsVerticallyHomogeneous()));
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
         data.Add(new Snoop.Data.Object("Supporting Element", GetElementById(supportData.GetSupportingElement())));
         data.Add(new Snoop.Data.Object("Curve", supportData.GetCurve()));
         data.Add(new Snoop.Data.Object("Face", supportData.GetFace()));
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

      private void Stream(ArrayList data, Autodesk.Revit.DB.StructuralAsset asset)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(Autodesk.Revit.DB.StructuralAsset)));

         data.Add(new Snoop.Data.String("Behavior", asset.Behavior.ToString()));
         data.Add(new Snoop.Data.Double("ConcreteBendingReinforcement ", asset.ConcreteBendingReinforcement));
         data.Add(new Snoop.Data.Double("ConcreteCompression ", asset.ConcreteCompression));
         data.Add(new Snoop.Data.Double("ConcreteShearReinforcement ", asset.ConcreteShearReinforcement));
         data.Add(new Snoop.Data.Double("ConcreteShearStrengthReduction ", asset.ConcreteShearStrengthReduction));
         data.Add(new Snoop.Data.Double("DampingRatio ", asset.DampingRatio));
         data.Add(new Snoop.Data.Bool("Lightweight ", asset.Lightweight));
         data.Add(new Snoop.Data.Double("MinimumTensileStrength ", asset.MinimumTensileStrength));
         data.Add(new Snoop.Data.Double("MinimumYieldStress ", asset.MinimumYieldStress));
         data.Add(new Snoop.Data.String("Name ", asset.Name));

         data.Add(new Snoop.Data.Xyz("PoissonRatio ", asset.PoissonRatio));
         data.Add(new Snoop.Data.Xyz("ShearModulus ", asset.ShearModulus));
         data.Add(new Snoop.Data.Double("MetalReductionFactor ", asset.MetalReductionFactor));
         data.Add(new Snoop.Data.Double("MetalResistanceCalculationStrength ", asset.MetalResistanceCalculationStrength));
         data.Add(new Snoop.Data.Double("MinimumYieldStress ", asset.MinimumYieldStress));
         data.Add(new Snoop.Data.String("StructuralAssetClass", asset.StructuralAssetClass.ToString()));
         data.Add(new Snoop.Data.String("SubClass ", asset.SubClass));
         data.Add(new Snoop.Data.Xyz("ThermalExpansionCoefficient ", asset.ThermalExpansionCoefficient));
 //        data.Add(new Snoop.Data.Double("UnitWeight (obsolete)", asset.UnitWeight));
         data.Add(new Snoop.Data.Double("Density ", asset.Density));

         data.Add(new Snoop.Data.Double("WoodBendingStrength ", asset.WoodBendingStrength));
         data.Add(new Snoop.Data.String("WoodGrade ", asset.WoodGrade));
         data.Add(new Snoop.Data.Double("WoodParallelCompressionStrength ", asset.WoodParallelCompressionStrength));
         data.Add(new Snoop.Data.Double("WoodParallelShearStrength ", asset.WoodParallelShearStrength));
         data.Add(new Snoop.Data.Double("WoodPerpendicularCompressionStrength ", asset.WoodPerpendicularCompressionStrength));
         data.Add(new Snoop.Data.Double("WoodPerpendicularShearStrength ", asset.WoodPerpendicularShearStrength));
         data.Add(new Snoop.Data.String("WoodSpecies) ", asset.WoodSpecies));

         data.Add(new Snoop.Data.Xyz("YoungModulus ", asset.YoungModulus));
      }
/* TF BUGBUG??
      private void
      Stream(ArrayList data, AnalyticalModelSweptProfile profile)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(AnalyticalModelSweptProfile)));

         data.Add(new Snoop.Data.Object("Driving curve", profile.GetDrivingCurve()));
         data.Add(new Snoop.Data.Object("Profile", profile.GetSweptProfile()));

         data.Add(new Snoop.Data.Double("Start Set Back ", profile.StartSetBack));
         data.Add(new Snoop.Data.Double("End Set Back ", profile.EndSetBack));         
      }
 */
   }
}
