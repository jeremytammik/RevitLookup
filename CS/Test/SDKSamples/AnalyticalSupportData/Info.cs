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
using System.Data;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;


namespace RevitLookup.Test.SDKSamples.AnalyticalSupportData
{

    /// <summary>
    /// Class to cache info about Analytical Support Data
    /// </summary>

   public class Info
   {

      DataTable m_elementInformation = null;  // store all required information

      public
      //Info( ElementSet selectedElements ) // 2015, jeremy
      Info( Document doc, ICollection<ElementId> selectedElementIds ) // 2016, jeremy
      {
         m_elementInformation = StoreInformationInDataTable(doc,selectedElementIds);
      }

      /// <summary>
      /// property to get private member variable m_elementInformation.
      /// </summary>

      public DataTable
      ElementInformation
      {
         get { return m_elementInformation; }
      }

      /// <summary>
      /// get all the required information of selected elements and store them in a data table
      /// </summary>
      /// <param name="selectedElements">
      /// all selected elements in Revit main program
      /// </param>
      /// <returns>
      /// a data table which store all the required information
      /// </returns>

      //private DataTable StoreInformationInDataTable(ElementSet selectedElements) // 2015, jeremy
      private DataTable StoreInformationInDataTable(Document doc, ICollection<ElementId> selectedElementIds) // 2016, jeremy
      {
         DataTable informationTable = CreateDataTable();
         informationTable.BeginLoadData();

         string typeName = "";
         string infoStr1 = "";
         string infoStr2 = "";
         List<AnalyticalModelSupport> analyticalModelSupports = new List<AnalyticalModelSupport>();
         //bool getInformationflag;

         foreach (ElementId id in selectedElementIds)
         {
           Element element = doc.GetElement( id );

            typeName = string.Empty;
            //getInformationflag = false;

            AnalyticalModel am = element.GetAnalyticalModel();
            if (null == am)
            {
               continue;
            }

            analyticalModelSupports = am.GetAnalyticalModelSupports() as List<AnalyticalModelSupport>;

            GetSupportInformation(analyticalModelSupports, ref infoStr1, ref infoStr2);

            // Add a new row with the information
            DataRow newRow = informationTable.NewRow();
            newRow["Id"] = element.Id.IntegerValue.ToString();
            newRow["Element Type"] = typeName;
            newRow["Support Type"] = infoStr1;
            newRow["Remark"] = infoStr2;
            informationTable.Rows.Add(newRow);
         }

         informationTable.EndLoadData();
         return informationTable;
      }

      /// <summary>
      /// Create an empty dataTable
      /// </summary>
      /// <returns></returns>

      private DataTable
      CreateDataTable()
      {
         // Create a new DataTable.
         DataTable elementInformationTable = new DataTable("ElementInformationTable");

         // Create element id column and add to the DataTable.
         DataColumn idColumn = new DataColumn();
         idColumn.DataType = typeof(System.String);
         idColumn.ColumnName = "Id";
         idColumn.Caption = "Id";
         idColumn.ReadOnly = true;
         elementInformationTable.Columns.Add(idColumn);

         // Create element type column and add to the DataTable.
         DataColumn typeColumn = new DataColumn();
         typeColumn.DataType = typeof(System.String);
         typeColumn.ColumnName = "Element Type";
         typeColumn.Caption = "Element Type";
         typeColumn.ReadOnly = true;
         elementInformationTable.Columns.Add(typeColumn);

         // Create support column and add to the DataTable.
         DataColumn supportColumn = new DataColumn();
         supportColumn.DataType = typeof(System.String);
         supportColumn.ColumnName = "Support Type";
         supportColumn.Caption = "Support Type";
         supportColumn.ReadOnly = true;
         elementInformationTable.Columns.Add(supportColumn);

         // Create a colum which can note others information
         DataColumn remarkColumn = new DataColumn();
         remarkColumn.DataType = typeof(System.String);
         remarkColumn.ColumnName = "Remark";
         remarkColumn.Caption = "Remark";
         remarkColumn.ReadOnly = true;
         elementInformationTable.Columns.Add(remarkColumn);


         return elementInformationTable;
      }

      /// <summary>
      /// get element's support information
      /// </summary>
      /// <param name="supportData"> element's support data</param>
      /// <returns></returns>
      private void GetSupportInformation(List<AnalyticalModelSupport> supports, ref string str1, ref string str2)
      {
         str1 = string.Empty;
         str2 = string.Empty;

         if (null == supports || 0 == supports.Count)
         {
            str1 = "not supported";
         }
         else
         {
            foreach (AnalyticalModelSupport support in supports)
            {
               str1 = str1 + support.GetSupportType().ToString() + ", ";
            }
         }
      }
   }
}
