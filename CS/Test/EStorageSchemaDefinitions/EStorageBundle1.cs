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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitLookup.Test
{
   /// <summary>
   /// A simple EStorage schema/entity bundle containing one field.
   /// </summary>
   public class EStorageBundle_1 : EStorageBundle
   {
      public EStorageBundle_1(string schemaName, Guid schemaGuid)
         : base(schemaName, schemaGuid)
      {

      }

      public override Schema GetOrCreateSchema()
      {
         SchemaBuilder sb = new SchemaBuilder(SchemaGuid);
         sb.SetSchemaName(SchemaName);
         sb.AddSimpleField("someBool", typeof(bool));
         return sb.Finish();
      }

      public override Entity CreateEntityOfSchema()
      {
         Entity entity = new Entity(GetOrCreateSchema());
         entity.Set("someBool", true);
         return entity;
      }
   }


   partial class TestEStorage
   {
      /// <summary>
      /// Add an entity from EStorageBundle_1 to all selected elements.
      /// </summary>
      public void AddSchema1andEntity()
      {
         AddSchemaAndEntityImplementation(new EStorageBundle_1("TestSchema1", new Guid("{2F083E88-93FA-43B7-A0CF-7771DE447822}")));
      }

   }
}