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
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

using RevitLookup.Snoop.Collectors;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;



namespace RevitLookup.Snoop.CollectorExts
{

   public class CollectorExtMEP : CollectorExt
   {

      public
      CollectorExtMEP()
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
         Connector connector = e.ObjToSnoop as Connector;
         if (connector != null)
         {
            Utils.StreamWithReflection(snoopCollector.Data(), typeof(Connector), connector);
            return;
         }

         ConnectorManager connectorMgr = e.ObjToSnoop as ConnectorManager;
         if (connectorMgr != null)
         {
            Stream(snoopCollector.Data(), connectorMgr);
            return;
         }

         CorrectionFactor correctionFactor = e.ObjToSnoop as CorrectionFactor;
         if (correctionFactor != null)
         {
            Stream(snoopCollector.Data(), correctionFactor);
            return;
         }

         ElectricalSetting elecSetting = e.ObjToSnoop as ElectricalSetting;
         if (elecSetting != null)
         {
            Stream(snoopCollector.Data(), elecSetting);
            return;
         }

         GroundConductorSize groundConductorSize = e.ObjToSnoop as GroundConductorSize;
         if (groundConductorSize != null)
         {
            Stream(snoopCollector.Data(), groundConductorSize);
            return;
         }

         MEPModel mepModel = e.ObjToSnoop as MEPModel;
         if (mepModel != null)
         {
            Stream(snoopCollector.Data(), mepModel);
            return;
         }

         WireSize wireSize = e.ObjToSnoop as WireSize;
         if (wireSize != null)
         {
            Stream(snoopCollector.Data(), wireSize);
            return;
         }
      }

      private static void AddDataWithExceptionSafe(ArrayList data, String dataName, Double dataValue)
      {
         try
         {
            data.Add(new Snoop.Data.Double(dataName, dataValue));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception(dataName, ex));
         }
      }

      private static void AddDataWithExceptionSafe(ArrayList data, String dataName, String dataValue)
      {
         try
         {
            data.Add(new Snoop.Data.String(dataName, dataValue));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception(dataName, ex));
         }
      }

      private static void AddDataWithExceptionSafe(ArrayList data, String dataName, Object dataValue)
      {
         try
         {
            data.Add(new Snoop.Data.Object(dataName, dataValue));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception(dataName, ex));
         }
      }

      private static void AddDataWithExceptionSafe(ArrayList data, String dataName, XYZ dataValue)
      {
         try
         {
            data.Add(new Snoop.Data.Xyz(dataName, dataValue));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception(dataName, ex));
         }
      }

      private void
        Stream(ArrayList data, ConnectorManager connectorMgr)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ConnectorManager)));

         data.Add(new Snoop.Data.Enumerable("Connectors", connectorMgr.Connectors));
         data.Add(new Snoop.Data.Enumerable("Unused connectors", connectorMgr.UnusedConnectors));
         data.Add(new Snoop.Data.Object("Owner", connectorMgr.Owner));
      }

      private void
        Stream(ArrayList data, CorrectionFactor correctionFactor)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(CorrectionFactor)));

         data.Add(new Snoop.Data.Double("Factor", correctionFactor.Factor));
         data.Add(new Snoop.Data.Double("Temperature", correctionFactor.Temperature));
      }

      private void
        Stream(ArrayList data, MEPModel mepModel)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MEPModel)));

         data.Add(new Snoop.Data.Object("Connector manager", mepModel.ConnectorManager));
         data.Add(new Snoop.Data.Enumerable("Electrical systems", mepModel.ElectricalSystems));

         ElectricalEquipment elecEquip = mepModel as ElectricalEquipment;
         if (elecEquip != null)
         {
            Stream(data, elecEquip);
            return;
         }

         LightingDevice lightDevice = mepModel as LightingDevice;
         if (lightDevice != null)
         {
            Stream(data, lightDevice);
            return;
         }

         LightingFixture lightFixture = mepModel as LightingFixture;
         if (lightFixture != null)
         {
            Stream(data, lightFixture);
            return;
         }

         MechanicalEquipment mechEquip = mepModel as MechanicalEquipment;
         if (mechEquip != null)
         {
            Stream(data, mechEquip);
            return;
         }

         MechanicalFitting mechFitting = mepModel as MechanicalFitting;
         if (mechFitting != null)
         {
            Stream(data, mechFitting);
            return;
         }
      }

      private void
        Stream(ArrayList data, ElectricalEquipment elecEquip)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ElectricalEquipment)));

         data.Add(new Snoop.Data.Object("Distribution system", elecEquip.DistributionSystem));
      }

      private void
        Stream(ArrayList data, LightingDevice lightDevice)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LightingDevice)));

         // no data at this level
      }

      private void
        Stream(ArrayList data, LightingFixture lightFixture)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(LightingFixture)));

         // no data at this level
      }

      private void
        Stream(ArrayList data, MechanicalEquipment mechEquip)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MechanicalEquipment)));

         // no data at this level
      }

      private void
      Stream(ArrayList data, MechanicalFitting mechFitting)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(MechanicalFitting)));

         data.Add(new Snoop.Data.String("Part type", mechFitting.PartType.ToString()));
      }

      private void
        Stream(ArrayList data, ElectricalSetting elecSetting)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(ElectricalSetting)));

         data.Add(new Snoop.Data.Enumerable("Conduit types", elecSetting.WireConduitTypes));
         data.Add(new Snoop.Data.Enumerable("Distribution system types", elecSetting.DistributionSysTypes));
         data.Add(new Snoop.Data.Enumerable("Voltage types", elecSetting.VoltageTypes));
         data.Add(new Snoop.Data.Enumerable("Wire material types", elecSetting.WireMaterialTypes));
         data.Add(new Snoop.Data.Enumerable("Wire types", elecSetting.WireTypes));

         data.Add(new Snoop.Data.CategorySeparator("Demand Factors"));
      }

      private void
        Stream(ArrayList data, GroundConductorSize grndCondSize)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(GroundConductorSize)));

         data.Add(new Snoop.Data.Double("Ampacity", grndCondSize.Ampacity));
         data.Add(new Snoop.Data.String("Conductor size", grndCondSize.ConductorSize));
         data.Add(new Snoop.Data.Object("Material belong to", grndCondSize.MaterialBelongTo));
      }

      private void
        Stream(ArrayList data, WireSize wireSize)
      {
         data.Add(new Snoop.Data.ClassSeparator(typeof(WireSize)));

         data.Add(new Snoop.Data.Double("Ampacity", wireSize.Ampacity));
         data.Add(new Snoop.Data.Double("Diameter", wireSize.Diameter));
         data.Add(new Snoop.Data.Bool("In use", wireSize.InUse));
         data.Add(new Snoop.Data.String("Size", wireSize.Size));
      }
   }
}
