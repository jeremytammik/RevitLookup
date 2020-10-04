#region Header
//
// Copyright 2003-2020 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;


using Autodesk.Revit.DB;

using RevitLookup.ModelStats;

namespace RevitLookup.ModelStats
{
  /// <summary>
  /// Summary description for Report.
  /// </summary>
  public class Report
  {
    // data members
    private ArrayList m_rawObjCounts = new ArrayList();
    private ArrayList m_categoryCounts = new ArrayList();
    private ArrayList m_symRefCounts = new ArrayList();

    public
    Report()
    {
    }

    /// <summary>
    /// Find a RawObjCount node for the the given type of object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>

    private RawObjCount
    FindRawObjNode( System.Type objType )
    {
      foreach( RawObjCount tmpNode in m_rawObjCounts )
      {
        if( tmpNode.m_classType == objType )
          return tmpNode;
      }
      return null;
    }

    private void
        RawObjStats( System.Object obj )
    {
      RawObjCount tmpNode = FindRawObjNode( obj.GetType() );
      if( tmpNode == null )
      {
        tmpNode = new RawObjCount();
        tmpNode.m_classType = obj.GetType();
        m_rawObjCounts.Add( tmpNode );
      }

      tmpNode.m_objs.Add( obj );
    }

    private CategoryCount
    FindCategoryNode( Category cat )
    {
      Debug.Assert( cat != null );    // don't call unless you've already checked this

      foreach( CategoryCount tmpNode in m_categoryCounts )
      {
        if( tmpNode.m_category == cat )
          return tmpNode;
      }

      return null;
    }

    private void
    CategoryStats( Element elem )
    {
      if( elem.Category == null )  // some elements don't belong to a category
        return;

      CategoryCount tmpNode = FindCategoryNode( elem.Category );
      if( tmpNode == null )
      {
        tmpNode = new CategoryCount();
        tmpNode.m_category = elem.Category;
        m_categoryCounts.Add( tmpNode );
      }

      tmpNode.m_objs.Add( elem );
    }

    private SymbolCount
FindSymbolNode( ElementType sym )
    {
      foreach( SymbolCount tmpNode in m_symRefCounts )
      {
        if( tmpNode.m_symbol.Id.IntegerValue == sym.Id.IntegerValue )  // TBD: directly comparing Symbol objects doesn't work
          return tmpNode;
      }
      return null;
    }

    private ElementType
    GetSymbolRef( Element elem )
    {
      FamilyInstance famInst = elem as FamilyInstance;
      if( famInst != null )
        return famInst.Symbol;

      Floor floor = elem as Floor;
      if( floor != null )
        return floor.FloorType;

      Group group = elem as Group;
      if( group != null )
        return group.GroupType;

      Wall wall = elem as Wall;
      if( wall != null )
        return wall.WallType;

      return null;    // nothing we know about
    }

    private void
    SymbolRefStats( Element elem )
    {
      // if it is a Symbol element, just make an entry in our map
      // and get out.
      ElementType sym = elem as ElementType;
      if( sym != null )
      {
        SymbolCount tmpNode = FindSymbolNode( sym );
        if( tmpNode == null )
        {
          tmpNode = new SymbolCount();
          tmpNode.m_symbol = sym;
          m_symRefCounts.Add( tmpNode );
        }

        return;
      }

      // it isn't a Symbol, so we need to check if it is something that
      // references a Symbol.
      sym = GetSymbolRef( elem );
      if( sym != null )
      {
        SymbolCount tmpNode = FindSymbolNode( sym );
        if( tmpNode == null )
        {
          tmpNode = new SymbolCount();
          tmpNode.m_symbol = sym;
          m_symRefCounts.Add( tmpNode );
        }
        tmpNode.m_refs.Add( elem );
      }
    }


    private void
    ProcessElements( Document doc )
    {
      FilteredElementCollector fec = new FilteredElementCollector( doc );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Element ) );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        RawObjStats( element );

        if( element != null )
        {
          SymbolRefStats( element );
          CategoryStats( element );
        }
      }
    }

    /// <summary>
    /// Create the XML Report for this document
    /// </summary>
    /// <param name="reportPath"></param>
    /// <param name="elemIter"></param>

    public void
    XmlReport( string reportPath, Document doc )
    {
      ProcessElements( doc );   // index all of the elements

      XmlTextWriter stream = new XmlTextWriter( reportPath, System.Text.Encoding.UTF8 );
      stream.Formatting = Formatting.Indented;
      stream.IndentChar = '\t';
      stream.Indentation = 1;

      stream.WriteStartDocument();

      stream.WriteStartElement( "Project" );
      stream.WriteAttributeString( "title", doc.Title );
      stream.WriteAttributeString( "path", doc.PathName );

      XmlReportRawCounts( stream );
      XmlReportCategoryCounts( stream );
      XmlReportSymbolRefCounts( stream );

      stream.WriteEndElement();   // "Project"
      stream.WriteEndDocument();

      stream.Close();
    }

    private void
    XmlReportRawCounts( XmlTextWriter stream )
    {
      stream.WriteStartElement( "RawCounts" );

      foreach( RawObjCount tmpNode in m_rawObjCounts )
      {
        // write summary stats for this class type
        stream.WriteStartElement( "ClassType" );
        stream.WriteAttributeString( "name", tmpNode.m_classType.Name );
        stream.WriteAttributeString( "fullName", tmpNode.m_classType.FullName );
        stream.WriteAttributeString( "count", tmpNode.m_objs.Count.ToString() );
        // list a reference to each element of this type
        foreach( System.Object tmpObj in tmpNode.m_objs )
        {
          Element tmpElem = tmpObj as Element;
          if( tmpElem != null )
          {
            stream.WriteStartElement( "ElementRef" );
            stream.WriteAttributeString( "idRef", tmpElem.Id.IntegerValue.ToString() );
            stream.WriteEndElement();   // ElementRef
          }
        }

        stream.WriteEndElement();   // ClassType
      }

      stream.WriteEndElement();   // RawCounts
    }

    private void
    XmlReportCategoryCounts( XmlTextWriter stream )
    {
      stream.WriteStartElement( "Categories" );

      foreach( CategoryCount tmpNode in m_categoryCounts )
      {
        // write summary stats for this category
        stream.WriteStartElement( "Category" );
        stream.WriteAttributeString( "name", tmpNode.m_category.Name );
        stream.WriteAttributeString( "count", tmpNode.m_objs.Count.ToString() );
        // list a reference to each element of this type
        foreach( Element tmpElem in tmpNode.m_objs )
        {
          stream.WriteStartElement( "ElementRef" );
          stream.WriteAttributeString( "idRef", tmpElem.Id.IntegerValue.ToString() );
          stream.WriteEndElement();   // ElementRef
        }

        stream.WriteEndElement();   // Category
      }

      stream.WriteEndElement();   // Categories
    }

    private void
    XmlReportSymbolRefCounts( XmlTextWriter stream )
    {
      stream.WriteStartElement( "Symbols" );

      foreach( SymbolCount tmpNode in m_symRefCounts )
      {
        // write summary stats for this Symbol
        stream.WriteStartElement( "Symbol" );
        stream.WriteAttributeString( "name", tmpNode.m_symbol.Name );
        stream.WriteAttributeString( "symbolType", tmpNode.m_symbol.GetType().Name );
        stream.WriteAttributeString( "refCount", tmpNode.m_refs.Count.ToString() );
        // list a reference to each element of this type
        foreach( Element tmpElem in tmpNode.m_refs )
        {
          stream.WriteStartElement( "ElementRef" );
          stream.WriteAttributeString( "idRef", tmpElem.Id.IntegerValue.ToString() );
          stream.WriteEndElement();   // ElementRef
        }

        stream.WriteEndElement();   // Symbol
      }

      stream.WriteEndElement();   // Symbols
    }

    private void
    XmlReportWriteElement( XmlTextWriter stream, Element elem )
    {
      if( elem.Category == null )
        return;

      stream.WriteStartElement( "BldgElement" );

      stream.WriteAttributeString( "category", elem.Category.Name );
      stream.WriteAttributeString( "id", elem.Id.IntegerValue.ToString() );
      stream.WriteAttributeString( "name", elem.Name );

      ParameterSet paramSet = elem.Parameters;
      foreach( Parameter tmpObj in paramSet )
      {
        stream.WriteStartElement( "Param" );
        stream.WriteAttributeString( "name", tmpObj.Definition.Name );
        if( tmpObj.StorageType == StorageType.Double )
        {
          stream.WriteAttributeString( "dataType", "double" );
          stream.WriteAttributeString( "value", tmpObj.AsDouble().ToString() );
        }
        else if( tmpObj.StorageType == StorageType.ElementId )
        {
          stream.WriteAttributeString( "dataType", "elemId" );
          stream.WriteAttributeString( "value", tmpObj.AsElementId().IntegerValue.ToString() );
        }
        else if( tmpObj.StorageType == StorageType.Integer )
        {
          stream.WriteAttributeString( "dataType", "int" );
          stream.WriteAttributeString( "value", tmpObj.AsInteger().ToString() );
        }
        else if( tmpObj.StorageType == StorageType.String )
        {
          stream.WriteAttributeString( "dataType", "string" );
          stream.WriteAttributeString( "value", tmpObj.AsString() );
        }
        else if( tmpObj.StorageType == StorageType.None )
        {
          stream.WriteAttributeString( "dataType", "none" );
          stream.WriteAttributeString( "value", "???" );
        }
        stream.WriteEndElement();   // "Param"
      }

      stream.WriteEndElement();   // "BldgElement"
    }

  }
}
