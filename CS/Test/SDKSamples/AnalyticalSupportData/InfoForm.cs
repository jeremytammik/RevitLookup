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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RevitLookup.Test.SDKSamples.AnalyticalSupportData
{
    /// <summary>
    /// UI which displays the information
    /// </summary>
    public partial class InfoForm : Form
    {
        
        public
        InfoForm(AnalyticalSupportData.Info supportData)
        {
            InitializeComponent();

            SetDataTableSource(supportData);
        }
 
        private void
        SetDataTableSource(AnalyticalSupportData.Info supportData)
        {
                // set data source
            m_dataGrid.AutoGenerateColumns = false;
            m_dataGrid.DataSource          = supportData.ElementInformation;
            
                // match column names with data source names
            m_colId.DataPropertyName          = "Id";
            m_colTypeName.DataPropertyName    = "Element Type";
            m_colSupportType.DataPropertyName = "Support Type";
            m_colRemark.DataPropertyName      = "Remark";
        }

    }
}