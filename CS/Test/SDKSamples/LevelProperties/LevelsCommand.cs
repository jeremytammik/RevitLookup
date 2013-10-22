//
// (C) Copyright 1994-2006 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System;
using System.Collections.Generic;
using System.Text;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace RevitLookup.Test.SDKSamples.LevelProperties
{
    public class LevelsCommand
    {
        Autodesk.Revit.UI.UIApplication m_app = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public LevelsCommand(Autodesk.Revit.UI.UIApplication app)
        {
            m_app = app;
        }

        #region GetDatum

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean
        Execute ()
        {
            //Get every level by iterating through all elements
            systemLevelsData = new List<LevelsDataSource>();

            FilteredElementCollector fec = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
            ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(Level));
            fec.WherePasses(elementsAreWanted);
            List<Element> elements = fec.ToElements() as List<Element>;

            foreach (Element element in elements)
            {
               Level systemLevel = element as Level;
               if (systemLevel != null)
               {
                  LevelsDataSource levelsDataSourceRow = new LevelsDataSource();

                  levelsDataSourceRow.LevelIDValue = systemLevel.Id.IntegerValue;
                  levelsDataSourceRow.Name = systemLevel.Name;

                  levelsDataSourceRow.Elevation = systemLevel.Elevation;

                  systemLevelsData.Add(levelsDataSourceRow);
               }
            }

            LevelsForm displayForm = new LevelsForm(this);
            displayForm.ShowDialog();

            return true;
        }

        //Store all levels's datum in system
        List<LevelsDataSource> systemLevelsData;

        /// <summary>
        /// 
        /// </summary>
        public List<LevelsDataSource> SystemLevelsData
        {
            get
            {
                return systemLevelsData;
            }
            set
            {
                systemLevelsData = value;
            }
        }

        #endregion

        #region SetData
        /// <summary>
        /// Set Level
        /// </summary>
        /// <param name="levelIDValue">Pass a Level's ID value</param>
        /// <param name="levelName">Pass a Level's Name</param>
        /// <param name="levelElevation">Pass a Level's Elevation</param>
        public void SetLevel (int levelIDValue, String levelName, double levelElevation)
        {
           FilteredElementCollector fec = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
           ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(Level));
           fec.WherePasses(elementsAreWanted);
           List<Element> elements = fec.ToElements() as List<Element>;

           foreach (Element element in elements)
           {
              Level systemLevel = element as Autodesk.Revit.DB.Level;
              if (systemLevel != null)
              {
                 if (systemLevel.Id.IntegerValue == levelIDValue)
                 {
                    systemLevel.Name = levelName;
                    systemLevel.Elevation = levelElevation;
                 }
              }
           }

        }
        #endregion

        #region CreateLevel
        /// <summary>
        /// Create a level
        /// </summary>
        /// <param name="levelName">Pass a Level's Name</param>
        /// <param name="levelElevation">Pass a Level's Elevation</param>
        public void CreateLevel (String levelName, double levelElevation)
        {
            Level newLevel = m_app.ActiveUIDocument.Document.Create.NewLevel(levelElevation);

            newLevel.Name = levelName;
        }
        #endregion

        #region DeleteLevel
        /// <summary>
        /// Delete a Level.
        /// </summary>
        /// <param name="IDValueOfLevel">A Level's ID value</param>
        public void DeleteLevel (int IDValueOfLevel)
        {
            ElementId IDOfLevel = new ElementId(IDValueOfLevel);            

            m_app.ActiveUIDocument.Document.Delete(IDOfLevel);
        }
        #endregion
    }
}
