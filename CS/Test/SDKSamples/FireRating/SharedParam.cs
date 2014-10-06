#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Windows.Forms;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace RevitLookup.Test.SDKSamples.FireRating {

    class SharedParam {

        Autodesk.Revit.UI.UIApplication m_app = null;
        
        public const string m_sharedParamFile     = "RevitLookup.txt";   // use this name if there isn't already a shared param file
        public const string m_paramGroupName      = "RevitLookup";
        public const string m_fireRatingParamName = "Fire Rating";

        public
        SharedParam(Autodesk.Revit.UI.UIApplication app)
        {
            m_app = app;
        }

        public void
        AddSharedParamToFile()
        {
                // open the file
            DefinitionFile sharedParametersFile = null;
            try {
                sharedParametersFile = OpenSharedParamFile();
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format("Can't open Shared Parameters file. (Exception: {0})", ex.Message));
                return;
            }

                // get or create our group
            DefinitionGroup sharedParameterGroup = sharedParametersFile.Groups.get_Item(m_paramGroupName);  // "RevitLookup"
            if (sharedParameterGroup == null) {
                sharedParameterGroup = sharedParametersFile.Groups.Create(m_paramGroupName);
            }

                // get or create the parameter
            Definition sharedParameterDefinition = sharedParameterGroup.Definitions.get_Item(m_fireRatingParamName);    // "Fire Rating"
            if (sharedParameterDefinition == null) {
              //sharedParameterDefinition = sharedParameterGroup.Definitions.Create( m_fireRatingParamName, ParameterType.Integer, true ); // 2015, jeremy: 'Autodesk.Revit.DB.Definitions.Create(string, Autodesk.Revit.DB.ParameterType, bool)' is obsolete: 'This method is deprecated in Revit 2015. Use Create(Autodesk.Revit.DB.ExternalDefinitonCreationOptions) instead'
              ExternalDefinitonCreationOptions opt = new ExternalDefinitonCreationOptions( m_fireRatingParamName, ParameterType.Integer );
              opt.Visible = true;
              sharedParameterDefinition = sharedParameterGroup.Definitions.Create( opt ); // 2016, jeremy

            }

            // create a category set with the Door category in it
            CategorySet categories = m_app.Application.Create.NewCategorySet();
            Category doorCategory = m_app.ActiveUIDocument.Document.Settings.Categories.get_Item("Doors");
            categories.Insert(doorCategory);

                // create a new instance binding for the wall categories
            InstanceBinding instanceBinding = m_app.Application.Create.NewInstanceBinding(categories);

                // add the binding
            m_app.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParameterDefinition, instanceBinding);

            MessageBox.Show("Added a shared parameter \"Fire Rating\" that applies to Category \"Doors\".");
        }

        public DefinitionFile
        OpenSharedParamFile()
        {
                // if no shared param file set up, make one
            if (m_app.Application.SharedParametersFilename == string.Empty) {
                StreamWriter stream = new StreamWriter(m_sharedParamFile);
                stream.Close();
                m_app.Application.SharedParametersFilename = m_sharedParamFile;
            }
            
            return m_app.Application.OpenSharedParameterFile();
        }
        
        public bool
        FindGUID(ref Guid paramGuid)
        {
            DefinitionFile sharedParamFile = null;
            try {
                sharedParamFile = OpenSharedParamFile();
                DefinitionGroup group = sharedParamFile.Groups.get_Item(m_paramGroupName);
                if (group != null) {
                    Definition definition = group.Definitions.get_Item(m_fireRatingParamName);
                    if (definition != null) {
                        ExternalDefinition externalDefinition = (ExternalDefinition)definition;
                        paramGuid = externalDefinition.GUID;
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception) {
                return false;
            }
       }


    }
}
