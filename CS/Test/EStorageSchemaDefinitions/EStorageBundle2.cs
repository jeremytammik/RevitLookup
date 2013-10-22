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
   public class EStorageBundle_2 : EStorageBundle
   {
      public EStorageBundle_2(string schemaName, Guid schemaGuid)
         : base(schemaName, schemaGuid)
      {

      }

      public override Schema GetOrCreateSchema()
      {
         SchemaBuilder sb = new SchemaBuilder(SchemaGuid);
         sb.SetSchemaName(SchemaName);
         sb.AddArrayField("someStringArray", typeof(string));
         return sb.Finish();
      }

      public override Entity CreateEntityOfSchema()
      {
         Entity entity = new Entity(GetOrCreateSchema());
         IList<String> stringSet = new List<string>() as IList<string>;
         stringSet.Add("abc");
         stringSet.Add("123");
         stringSet.Add("def");
         entity.Set("someStringArray", stringSet);
         return entity;
      }
   }


   partial class TestEStorage
   {
      /// <summary>
      /// Add an entity from EStorageBundle_2 to all selected elements.
      /// </summary>
      public void AddSchema2andEntity()
      {
         AddSchemaAndEntityImplementation(new EStorageBundle_2("TestSchema2", new Guid("{8C833F83-B862-413D-A57C-E163A0356CA9}")));
      }

   }
}