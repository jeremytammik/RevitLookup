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

namespace RevitLookup.Test.SDKSamples.LevelProperties
{
    /// <summary>
    /// Data source used to store all Level
    /// </summary>
    public class LevelsDataSource
    {
        String m_levelName;
        Double m_levelElevation;
        Int32 m_levelIDValue;

        /// <summary>
        /// First column used to store Level's Name
        /// </summary>
        public String Name
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }


        /// <summary>
        /// Second column to store Level's Elevation
        /// </summary>
        public double Elevation
        {
            get
            {
                return m_levelElevation;
            }
            set
            {
                m_levelElevation = value;
            }
        }

        /// <summary>
        /// Record Level's ID
        /// </summary>
        public int LevelIDValue
        {
            get
            {
                return m_levelIDValue;
            }
            set
            {
                m_levelIDValue = value;
            }
        }

    }
}
