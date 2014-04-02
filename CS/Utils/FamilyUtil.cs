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
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitLookup.Utils
{
  class FamilyUtil
  {
    public static FamilySymbol
    GetFamilySymbol( Autodesk.Revit.DB.Family fam, string famSymName )
    {
      // jeremy migrated from Revit 2014 to 2015:
      //FamilySymbolSetIterator famSymSetIter = fam.Symbols.ForwardIterator(); // 'Autodesk.Revit.DB.Family.Symbols' is obsolete: 'This property is obsolete in Revit 2015.  Use Family.GetFamilySymbolIds() instead.'
      //while( famSymSetIter.MoveNext() )
      //{
      //  FamilySymbol famSymTemp = famSymSetIter.Current as FamilySymbol;

      Document doc = fam.Document;

      foreach( ElementId id in fam.GetFamilySymbolIds() )
      {
        FamilySymbol famSymTemp = doc.GetElement( id )
          as FamilySymbol;

        if( famSymTemp != null )
        {
          if( famSymTemp.Name == famSymName )
          {
            return famSymTemp;
          }
        }
      }
      return null;
    }
  }
}
