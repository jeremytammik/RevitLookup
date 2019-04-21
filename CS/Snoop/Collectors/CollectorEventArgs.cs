#region Header
//
// Copyright 2003-2019 by Autodesk, Inc. 
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

namespace RevitLookup.Snoop.Collectors
{
	public class CollectorEventArgs : System.EventArgs
	{
	    private object  m_objToSnoop;
        private List<Type> seenTypes;
	    
		public CollectorEventArgs(object objToSnoop)
		{
		    m_objToSnoop = objToSnoop;
            seenTypes = new List<Type>();
		}
		
		public object ObjToSnoop
		{
		    get { return m_objToSnoop; }
		}
	}
}
