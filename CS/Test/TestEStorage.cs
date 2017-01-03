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
   /// A base class to manage the creation of schemas and entities of those schemas.
   /// </summary>
   public abstract class EStorageBundle
   {

      /// <summary>
      /// Set the schema name and guid the same way for all EStorage bundles.
      /// </summary>
      public EStorageBundle(string schemaName, Guid schemaGuid)
      {
         m_SchemaName = schemaName;
         m_SchemaGuid = schemaGuid;
      }

      /// <summary>
      /// Override this method to create an EStorage Schema
      /// </summary>
      public abstract Schema GetOrCreateSchema();

      /// <summary>
      /// Override this method to create an Entity of the Schema created in GetOrCreateSchema.
      /// </summary>
      public abstract Entity CreateEntityOfSchema();

      #region Accessors
      public Guid SchemaGuid
      {
         get { return m_SchemaGuid; }
      }

      public string SchemaName
      {
         get { return m_SchemaName; }
      }
      #endregion

      #region Data
      private Guid m_SchemaGuid;
      private string m_SchemaName;
      #endregion

   }

   partial class TestEStorage : RevitLookupTestFuncs
   {

      /// <summary>
      /// Add methods to add entities to selected elements or delete them as necessary here.
      /// Note that each method that adds an entity and schema is implemented in a separate class in
      /// .\EStroageSchemaDefinitions.  See EStorageBundle1.cs for an example.
      /// </summary>
      /// <param name="app"></param>
      public TestEStorage(Autodesk.Revit.UI.UIApplication app)
         : base(app)
      {
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Add entity of Schema_1 to the current selection.", "Add entity of Schema_1 to the current selection", typeof(Entity), new RevitLookupTestFuncInfo.TestFunc(AddSchema1andEntity), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Add entity of Schema_2 to the current selection.", "Add entity of Schema_2 to the current selection", typeof(Entity), new RevitLookupTestFuncInfo.TestFunc(AddSchema2andEntity), RevitLookupTestFuncInfo.TestType.Modify));
    
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Delete all EStorage from the document.", "Delete all EStorage from the document", typeof(Entity), new RevitLookupTestFuncInfo.TestFunc(DeleteAllEStorage), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Delete all EStorage from the selected elements.", "Delete all EStorage from the selected elements", typeof(Entity), new RevitLookupTestFuncInfo.TestFunc(DeleteAllEStorageFromSelectedElements), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Delete all EStorage from the selected elements with prompting.", "Delete all EStorage from the selected elements with prompting.", typeof(Entity), new RevitLookupTestFuncInfo.TestFunc(DeleteAllEStorageFromSelectedElementsWithPrompting), RevitLookupTestFuncInfo.TestType.Modify));
      }

      /// <summary>
      /// Called by all methods defined partial classes in .\EStorageSchemaDefinitions.  See EStorageBundle1.cs for an example.
      /// </summary>
      /// <param name="bundle"></param>
      private void AddSchemaAndEntityImplementation(EStorageBundle bundle)
      {
         Transaction tAddEntity = new Transaction(m_revitApp.ActiveUIDocument.Document, "Add Entity of: " + bundle.SchemaName);
         tAddEntity.Start();
         foreach (ElementId id in m_revitApp.ActiveUIDocument.Selection.GetElementIds())
         {
            Element element = m_revitApp.ActiveUIDocument.Document.GetElement(id);
            Entity entity = bundle.CreateEntityOfSchema();
            element.SetEntity(entity);
         }
         tAddEntity.Commit();
      }

      /// <summary>
      /// Delete all EStorage from the document, even if the user does not have read/write permission for it.
      /// </summary>
      public void DeleteAllEStorage()
      {
         Transaction tDelete = new Transaction(m_revitApp.ActiveUIDocument.Document, "Delete all storage.");
         tDelete.Start();
         foreach (Schema schema in Schema.ListSchemas())
         {
            Schema.EraseSchemaAndAllEntities(schema, true);
         }
         tDelete.Commit();
      }

      /// <summary>
      /// Delete all EStorage from the document, even if the user does not have read/write permission for it.
      /// </summary>
      public void DeleteAllEStorageFromSelectedElements()
      {
         DeleteStorageFromElementImplementation(prompt: false);
      }

      public void DeleteAllEStorageFromSelectedElementsWithPrompting()
      {
         DeleteStorageFromElementImplementation(prompt: true);
      }

      /// <summary>
      /// Delete all EStorage from the document, even if the user does not have read/write permission for it.
      /// </summary>
      private void DeleteStorageFromElementImplementation(bool prompt)
      {
         Transaction tDeleteEntity = new Transaction(m_revitApp.ActiveUIDocument.Document, "Delete EStorage from Selected Elements.");
         bool storageExists = false;
         tDeleteEntity.Start();
         foreach (ElementId id in m_revitApp.ActiveUIDocument.Selection.GetElementIds())
         {
            Element element = m_revitApp.ActiveUIDocument.Document.GetElement(id);
            IList<Guid> guids = element.GetEntitySchemaGuids();
            foreach (Guid guid in guids)
            {
               Schema schema = Schema.Lookup(guid);
               storageExists = true;
               if (prompt)
               {
                  DialogResult result = MessageBox.Show("Delete this Entity?" + Environment.NewLine + "Schema: " + schema.SchemaName + Environment.NewLine + "Guid: " + schema.GUID.ToString() + Environment.NewLine + "Vendor: " + schema.VendorId + Environment.NewLine + "Application: " + schema.ApplicationGUID.ToString(), "Delete EStorage?", MessageBoxButtons.YesNo);
                  if (result == DialogResult.Yes)
                  {
                     element.DeleteEntity(schema);
                  }
               }
               else
                  element.DeleteEntity(schema);

            }
         }
         if (!storageExists)
            MessageBox.Show("No Storage to delete.", "Delete EStorage");

         tDeleteEntity.Commit();

      }

   }
}