#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Diagnostics;
using System.Collections;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace RevitLookup.Utils
{

  public class Selection
  {

    /// <summary>
    /// Given an Enumerable set of Elements, walk through them and filter out the ones of a specific 
    /// category.
    /// </summary>
    /// <param name="set">The unfiltered set</param>
    /// <param name="filterForCategoryEnum">The BuiltInCategory enum to filter for</param>
    /// <param name="doc">The current Document object</param>
    /// <returns>The filtered ElementSet</returns>

    //public static ElementSet
    //FilterToCategory(IEnumerable set, BuiltInCategory filterForCategoryEnum, bool includeSymbols, Document doc) // 2015, jeremy

    public static ICollection<ElementId> FilterToCategory(
      ICollection<ElementId> set, 
      BuiltInCategory filterForCategoryEnum, 
      bool includeSymbols, 
      Document doc ) // 2016, jeremy
    {
      Category filterForCategory = doc.Settings.Categories.get_Item( filterForCategoryEnum );
      Debug.Assert( filterForCategory != null );

      //ElementSet filteredSet = new ElementSet();

      //IEnumerator iter = set.GetEnumerator();
      //while( iter.MoveNext() )
      //{
      //  Element elem = (Element) iter.Current;
      //  if( elem.Category == filterForCategory )
      //  {
      //    if( includeSymbols )
      //      filteredSet.Insert( elem );   // include it no matter what
      //    else
      //    {
      //      if( elem is ElementType == false )    // include it only if its not a symbol
      //        filteredSet.Insert( elem );
      //    }
      //  }
      //}

      //return filteredSet;

      List<ElementId> filteredSet = new List<ElementId>();

      foreach(ElementId id in set )
      {
        Element elem = doc.GetElement(id);
        if( elem.Category == filterForCategory )
        {
          if( includeSymbols // include it no matter what
            || !(elem is ElementType) )    // include it only if its not a symbol
          {
            filteredSet.Add( id );
          }
        }
      }

      return filteredSet;
    }

    /// <summary>
    /// Given an Enumerable set of Elements, walk through them and filter out the ones of a specific 
    /// category.
    /// </summary>
    /// <param name="filterForCategoryEnum">The BuiltInCategory enum to filter for</param>
    /// <param name="doc">The current Document object</param>
    /// <returns>The filtered ElementSet</returns>

    //public static ElementSet FilterToCategory( BuiltInCategory filterForCategoryEnum, bool includeSymbols, Document doc ) // 2015, jeremy

    public static ICollection<ElementId> FilterToCategory( 
      BuiltInCategory filterForCategoryEnum, 
      bool includeSymbols, 
      Document doc )
    {
      //ElementSet elemSet = new ElementSet();
      //FilteredElementCollector fec = new FilteredElementCollector( doc );
      //ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Element ) );
      //fec.WherePasses( elementsAreWanted );
      //List<Element> elements = fec.ToElements() as List<Element>;

      //foreach( Element element in elements )
      //{
      //  elemSet.Insert( element );
      //}
      //return FilterToCategory( elemSet, filterForCategoryEnum, includeSymbols, doc );

      ICollection<ElementId> ids = new FilteredElementCollector( doc ).OfClass( typeof( Element ) ).ToElementIds();
      return FilterToCategory( ids, filterForCategoryEnum, includeSymbols, doc );
    }

  }
}
