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
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace RevitLookup.Test.SDKSamples.SharedParams
{

   class Create
   {

      Autodesk.Revit.UI.UIApplication m_app = null;

      const string m_sharedParamFile = "RevitLookup.txt";   // use this name if there isn't already a shared param file
      const string m_paramGroupName = "RevitLookup";
      const string m_paramName = "Message from RevitLookup";

      public Create(Autodesk.Revit.UI.UIApplication app)
      {
         m_app = app;
      }

      public void
      AddSharedParamsToFile()
      {

         // open the file
         DefinitionFile sharedParametersFile = null;
         try
         {
            sharedParametersFile = OpenSharedParamFile();
         }
         catch (Exception ex)
         {
            MessageBox.Show(string.Format("Can't open Shared Parameters file. (Exception: {0})", ex.Message));
            return;
         }

         // get or create our group
         DefinitionGroup sharedParameterGroup = sharedParametersFile.Groups.get_Item(m_paramGroupName);
         if (sharedParameterGroup == null)
         {
            sharedParameterGroup = sharedParametersFile.Groups.Create(m_paramGroupName);
         }

         // get or create the parameter
         Definition sharedParameterDefinition = sharedParameterGroup.Definitions.get_Item(m_paramName);
         if (sharedParameterDefinition == null)
         {
            sharedParameterDefinition = sharedParameterGroup.Definitions.Create(m_paramName, ParameterType.Text, true);
         }

         // create a category set with the wall category in it
         CategorySet categories = m_app.Application.Create.NewCategorySet();
         Category wallCategory = m_app.ActiveUIDocument.Document.Settings.Categories.get_Item("Walls");
         categories.Insert(wallCategory);

         // create a new instance binding for the wall categories
         InstanceBinding instanceBinding = m_app.Application.Create.NewInstanceBinding(categories);

         // add the binding
         m_app.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParameterDefinition, instanceBinding);

         MessageBox.Show(string.Format("Added a shared parameter \"{0}\" that applies to Category \"Walls\".", m_paramName));
      }

      public void
      AddSharedParamsToWalls()
      {
         // open the file
         DefinitionFile sharedParametersFile = null;
         try
         {
            sharedParametersFile = m_app.Application.OpenSharedParameterFile();
         }
         catch (Exception ex)
         {
            MessageBox.Show(string.Format("Can't open Shared Parameters file. (Exception: {0})", ex.Message));
            return;
         }

         string mNewParamValue = "Hello, Hola, Mahalo, etc.";
         
         FilteredElementCollector fec = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
         ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(Element));
         fec.WherePasses(elementsAreWanted);
         List<Element> elements = fec.ToElements() as List<Element>;

         foreach (Element element in elements)
         {
            if (element is ElementType == false)
            {
               if (element.Category != null)
               {
                  if (element.Category.Name == "Walls")
                  {
                     foreach (Parameter param in element.Parameters)
                     {
                        if (param.Definition.Name == m_paramName)
                        {
                           param.Set(mNewParamValue);
                           break;
                        }
                     }
                  }
               }
            }
         }

         MessageBox.Show(string.Format("Added a shared parameter \"{0}\" to all Wall instances.", m_paramName));
      }

      public DefinitionFile
      OpenSharedParamFile()
      {
         // if no shared param file set up, make one
         if (m_app.Application.SharedParametersFilename == string.Empty)
         {
            StreamWriter stream = new StreamWriter(m_sharedParamFile);
            stream.Close();
            m_app.Application.SharedParametersFilename = m_sharedParamFile;
         }

         return m_app.Application.OpenSharedParameterFile();
      }

   }
}
