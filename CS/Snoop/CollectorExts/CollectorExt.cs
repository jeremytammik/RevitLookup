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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
	/// <summary>
	/// Base class for CollectorExt objects.
	/// </summary>
	public abstract class CollectorExt
	{
			// TBD: For the Snoop.Data.ElementId object I need access to the current
			// document so I can retrieve the Element.  However, from the context of something
			// like a Parameter.AsElementId(), the Document is nowhere to be found.  So, hack
			// around it for now by letting original TestCmd set this value.  Its not local-enough
			// when browsing though, so it could be wrong if browsing doesn't stay within the 
			// original document! (jma - 05/03/05)
        static public Autodesk.Revit.UI.UIApplication m_app = null;
        static public Autodesk.Revit.DB.Document m_activeDoc = null;

		public
		CollectorExt()
		{
		        // add ourselves to the event list of all SnoopCollectors
		    Snoop.Collectors.Collector.OnCollectorExt += new Snoop.Collectors.Collector.CollectorExt(CollectEvent);
            if (m_app != null && m_app.ActiveUIDocument != null && m_app.ActiveUIDocument.Document != null)
            {
                m_activeDoc = m_app.ActiveUIDocument.Document;
            }
		}
		
        protected abstract void
        CollectEvent(object sender, Snoop.Collectors.CollectorEventArgs e);

        public Element GetElementById(ElementId id)
        {
            if (m_activeDoc != null)
                return m_activeDoc.GetElement(id);
            return null;
        }
    }
}
