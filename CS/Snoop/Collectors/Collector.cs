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

namespace RevitLookup.Snoop.Collectors
{
	/// <summary>
    ///  This is the base class of data collector objects.  We'll just establish
    ///  the protocol for derived classes.  This class is abstract and cannot be
    ///  instantiated except by derived classes.  A Snoop.Collector class is passed
    ///  an object and manually fills up an ArrayList of Snoop.Data objects.  The list
    ///  is then displayed in a Form (or can be sent elsewhere if need be).  For the most
    ///  part, the Collector is just the initial catch point.  CollectorExts are responsible
    ///  for knowing about certain types of objects.
	/// </summary>
	
	public class Collector
	{
	        // Define an event so that we can broadcast out to any Collector Extension
	        // objects to fill in data that we don't know about.  The Event is static
	        // to the base class, so all instances of all derived classes will inherit
	        // the ability to be extended.
	    public delegate void CollectorExt(object sender, CollectorEventArgs e);
	    public static event  CollectorExt OnCollectorExt;
	    
	    protected ArrayList    m_dataObjs = new ArrayList();
	    
		public
		Collector()
		{
		}
		
		public ArrayList
		Data()
		{
		    return m_dataObjs;
		}
		
		    // Apparently, you can't call the Event from outside the actual class that defines it.
		    // So, we'll simply wrap it.  Now all derived classes can broadcast the event.
		protected void
		FireEvent_CollectExt(object objToSnoop)
		{
		    if (OnCollectorExt != null)
                OnCollectorExt(this, new CollectorEventArgs(objToSnoop));
        }
	}
}