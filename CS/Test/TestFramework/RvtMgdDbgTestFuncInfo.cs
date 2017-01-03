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
using System.Collections;


namespace RevitLookup.Test
{
	/// <summary>
	/// This is a simple package of data about a given Test function.  Create new instances
	/// of this object and add it to a RevitLookupTestFuncInfo-derived class.
	/// </summary>

    public class RevitLookupTestFuncInfo
    {         
        // so this enumerator will basically decide which type of test this is 
        // we might have to tweak this a litte bit for better results ...
        // the enum will be accessed by all the other tests to find out the best solution
        /// <summary>
        /// 
        /// </summary>
        public enum TestType
        {
            Create,
            Modify,
            Query,
            Other
        }

        public delegate void TestFunc ();

        private string          m_label           = null;		// label that will appear as the Test name. E.g., "Rotate Elements 45 degrees"
        private string          m_desc            = null;		// Description string (if you want something longer than the Label to explain it)
        private System.Type     m_classType       = null;		// What point of the class hierarchy does it test (E.g., only Walls or any Element)
        private TestFunc        m_testFunc        = null;		// The actual function to call to perform the test
        private string          m_categoryStr     = null;
        private Boolean         m_isCategoryBased = false;
        private TestType        m_testType;


        public
        RevitLookupTestFuncInfo (string label, string desc, System.Type classType, TestFunc func, TestType tType)
        {
            m_label = label;
            m_desc = desc;
            m_classType = classType;
            m_isCategoryBased = false;
            m_testFunc = func;
            m_testType = tType;
        } 

        public
        RevitLookupTestFuncInfo (string label, string desc, string categoryStr, TestFunc func, TestType tType)
        {
            m_label = label;
            m_desc = desc;
            m_classType = null;
            m_isCategoryBased = true;
            m_categoryStr = categoryStr;
            m_testFunc = func;
            m_testType = tType;
        }

        public string
        Label
        {
            get { return m_label; }
            set { m_label = value; }
        }

        public string
        Description
        {
            get { return m_desc; }
            set { m_desc = value; }
        }

        public System.Type
        ClassType
        {
            get { return m_classType; }
            set { m_classType = value; }
        }

        public void
        RunTest ()
        {
            if (m_testFunc != null)
            {
                m_testFunc();
            }
        }

        public string
        Category
        {
            get { return m_categoryStr; }
            set { m_categoryStr = value; }
        }

        public Boolean
        IsCategoryBased
        {
            get { return m_isCategoryBased; }
            set { m_isCategoryBased = value; }
        }          

        public TestType
        GetTestType ()
        {
            return m_testType;
        }
    }
}
