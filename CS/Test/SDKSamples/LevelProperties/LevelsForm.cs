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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RevitLookup.Test.SDKSamples.LevelProperties
{
    public partial class LevelsForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public LevelsForm ()
        {
            InitializeComponent();
        }

        //Class LevelsCommand's object reference
        LevelsCommand m_objectReference;

        //Record system total levels's
        Int32 m_systemLevelsTotal;

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opt">Used to get the LevelsCommand class's object.</param>
        public LevelsForm (LevelsCommand opt)
        {
            InitializeComponent();

            m_objectReference = opt;

            //Set control on UI
            LevelName = new DataGridViewTextBoxColumn();
            LevelName.HeaderText = "Name";
            LevelName.Width = 142;

            LevelElevation = new DataGridViewTextBoxColumn();
            LevelElevation.HeaderText = "Elevation";
            LevelElevation.Width = 142;

            levelsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { LevelName, LevelElevation });

            bindingSource1.DataSource = typeof(LevelsDataSource);
            //Must place below code on the code "dataGridView1.DataSource = bindingSource1"
            levelsDataGridView.AutoGenerateColumns = false;
            levelsDataGridView.DataSource = bindingSource1;
            LevelName.DataPropertyName = "Name";
            LevelElevation.DataPropertyName = "Elevation";

            //pass data to BindingSource
            
            bindingSource1.DataSource = m_objectReference.SystemLevelsData;

            //Record system levels's total
            m_systemLevelsTotal = m_objectReference.SystemLevelsData.Count;

            //Record changed items
            m_changedItemsFlag = new int[m_systemLevelsTotal];

            //Record deleted items
            m_deleteExistLevelIDValue = new int[m_systemLevelsTotal];
            m_deleteExistLevelTotal = 0;
        }        

        #endregion

        #region AddItem
        /// <summary>
        /// Used to add a new item in the dataGridView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click (object sender, EventArgs e)
        {
            System.String newLevelName;
            double newLevelElevation;
            
            if (bindingSource1.Count > 0)
            {
                bindingSource1.MoveLast();
                LevelsDataSource lastItem = bindingSource1.Current as LevelsDataSource;

                String lastLevelName = lastItem.Name;
                Double lastLevelElevation = lastItem.Elevation;
                newLevelName = lastLevelName;
                newLevelElevation = lastLevelElevation;
            }
            else
            {
                newLevelName = "Level" + " " + "1";
                newLevelElevation = 0;
            }


            LevelsDataSource newLevel = new LevelsDataSource();
            newLevel.Name = newLevelName;
            newLevel.Elevation = newLevelElevation;

            bindingSource1.Add(newLevel);
        }        
        #endregion

        #region RemoveItem
        /// <summary>
        /// Used to delete an item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click (object sender, EventArgs e)
        {
            if (bindingSource1.Position > m_systemLevelsTotal - 1)
            {
                bindingSource1.RemoveCurrent();
                return;
            }

            if (bindingSource1.Position <= m_systemLevelsTotal - 1 && bindingSource1.Position >= 0)
            {
                LevelsDataSource aRow = bindingSource1.Current as LevelsDataSource;
                m_deleteExistLevelIDValue[m_deleteExistLevelTotal] = aRow.LevelIDValue;
                m_deleteExistLevelTotal++;

                bindingSource1.RemoveCurrent();

                m_systemLevelsTotal = m_systemLevelsTotal - 1;

                int[] temChangedItemsFlag = new int[m_systemLevelsTotal];
                for (int i = 0, j = 0; i < m_systemLevelsTotal; i++, j++)
                {
                    if (bindingSource1.Position == i)
                    {
                        j++;
                    }
                    temChangedItemsFlag[i] = m_changedItemsFlag[j];
                }
                m_changedItemsFlag = temChangedItemsFlag;

                return;
            }

            if (bindingSource1.Position < 0)
            {
                System.Windows.Forms.MessageBox.Show("No levels present.");
            }
        }

        int[] m_deleteExistLevelIDValue;
        int m_deleteExistLevelTotal;
        #endregion

        #region CheckAndRecord
        /// <summary>
        /// Judge if the Name entered is unique.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_CellValidating (object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (0 == levelsDataGridView.CurrentCell.ColumnIndex)
            {
                System.String newName = e.FormattedValue as System.String;

                char[] newNameArray = new char[newName.Length];
                newNameArray = newName.ToCharArray();
                for (int i = 0; i < newName.Length; ++i)
                {
                    if ('\\' == newNameArray[i] || ':' == newNameArray[i] || '{' == newNameArray[i] ||
                        '}' == newNameArray[i] || '[' == newNameArray[i] || ']' == newNameArray[i] ||
                        '|' == newNameArray[i] || ';' == newNameArray[i] || '<' == newNameArray[i] ||
                        '>' == newNameArray[i] || '?' == newNameArray[i] || '`' == newNameArray[i] ||
                        '~' == newNameArray[i])
                    {
                        System.Windows.Forms.MessageBox.Show("Name cannot contain any of the following characters:\n\\ : { } [ ] | ; < > ? ` ~ \nor any of the non-printable characters.");
                        e.Cancel = true;

                        return;
                    }
                }

                System.String oldName = levelsDataGridView.CurrentCell.FormattedValue as System.String;
                if (newName != oldName)
                {
                    for (int i = 0; i < m_objectReference.SystemLevelsData.Count; i++)
                    {
                        if (m_objectReference.SystemLevelsData[i].Name == newName)
                        {
                            System.Windows.Forms.MessageBox.Show("The name entered is already in use. Enter a unique name.");
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Judge if the Elevation is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_DataError (object sender, DataGridViewDataErrorEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.Exception.Message);
        }

        /// <summary>
        /// Record the changed Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelsDataGridView_CellValueChanged (object sender, DataGridViewCellEventArgs e)
        {
            if (bindingSource1.Position < m_systemLevelsTotal)
            {
                m_systemLevelChangedFlag = 1;
                m_changedItemsFlag[bindingSource1.Position] = 1;
            }
        }

        //Record changed item
        int[] m_changedItemsFlag;
        int m_systemLevelChangedFlag = 0;
        #endregion

        #region OKButton
        /// <summary>
        /// Used to make setting apply to the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click (object sender, EventArgs e)
        {
            //Set all changed Level name's and elevation's
            if (1 == m_systemLevelChangedFlag)
            {
                for (int i = 0; i < m_changedItemsFlag.LongLength; i++)
                {
                    if (1 == m_changedItemsFlag[i])
                    {
                        bindingSource1.Position = i;
                        LevelsDataSource changeItem = bindingSource1.Current as LevelsDataSource;
                        m_objectReference.SetLevel(changeItem.LevelIDValue, changeItem.Name, changeItem.Elevation);
                    }
                }
            }

            //Delete existing Levels
            for (int i = 0; i < m_deleteExistLevelTotal; i++)
            {
                m_objectReference.DeleteLevel(m_deleteExistLevelIDValue[i]);
            }

            //Create new Levels
            for (int i = m_systemLevelsTotal; i < bindingSource1.Count; i++)
            {
                bindingSource1.Position = i;
                LevelsDataSource newItem = bindingSource1.Current as LevelsDataSource;
                m_objectReference.CreateLevel(newItem.Name, newItem.Elevation);
            }
        }
        #endregion
    }
}