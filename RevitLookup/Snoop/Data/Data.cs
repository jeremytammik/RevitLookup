#region Header
//
// Copyright 2003-2021 by Autodesk, Inc. 
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
using System.Windows.Forms;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// This is the base class for all types of "Snoop.Data".  Basically, we
	/// want something smarter than the original data type so that we can 
	/// hook it up to an editor and allow its output to go different places.
	/// </summary>
	
	public abstract class Data
	{
	    private string  m_label;
	    
		public
		Data(string label)
		{
		    m_label = label;
		}
		
		/// <summary>
		/// The Label value for the property (e.g. "Radius" for a Circle
		/// </summary>
		
		public virtual string
		Label
		{
            get { return m_label;  }
            set { m_label = value; }
   		}
		
		/// <summary>
		/// The value for the Property, expressed as a string
		/// </summary>
		/// <returns>The value formatted as a string</returns>
		
		public abstract string StrValue();
		
		/// <summary>
		/// Format the Label and Value as a single string.  The Snoop Forms will
		///	handle the Label/Value pair individually, but in other contexts, this
		///	could be used to make a flat list of Label/Value pairs.
		/// </summary>
		/// <returns>Label/Value pair as a string</returns>
		
		public override string ToString()
		{
		    return string.Format("{0}: {1}", m_label, StrValue());
		}
		
		/// <summary>
		/// Is there more information available about this property.  For instance,
		/// a type double would not have anything further to show.  But, a Collection
		/// can bring up a nested dialog showing all those objects.
		/// </summary>
		
        public virtual bool
        HasDrillDown
        {
            get { return false;  }
        }
		
		/// <summary>
		/// Do the act of drilling down on the data
		/// </summary>
		
        public virtual Form DrillDown()
        {
            return null;   // do nothing by default
        }
        
        /// <summary>
        /// Is this real data, or just a logical category separator?
        /// </summary>
        
        public virtual bool
        IsSeparator
        {
            get { return false;  }
        }

        /// <summary>
        /// Is this an error condition
        /// </summary>
        
        public virtual bool
        IsError
        {
            get { return false;  }
        }
    }
}
