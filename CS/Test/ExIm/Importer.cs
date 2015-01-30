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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Autodesk.Revit.DB;


using RevitLookup.Utils;

namespace RevitLookup.ExIm
{
  /// <summary>
  /// 
  /// </summary>
  public class Importer
  {
    /// <summary>
    /// 
    /// </summary>
    private string m_xmlFileName;
    private XmlDocument m_xmlDoc;
    private Document m_activeDoc;
    private ImportErrorLogger m_errorLogger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xmlDoc"></param>
    public Importer( XmlDocument xmlDoc, Document activeDoc )
    {
      m_xmlDoc = xmlDoc;
      m_activeDoc = activeDoc;
      m_errorLogger = new ImportErrorLogger( m_xmlFileName );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xmlDoc"></param>
    public Importer( string xmlFileName, Document activeDoc )
    {
      m_xmlFileName = xmlFileName;
      m_xmlDoc = new XmlDocument();
      m_xmlDoc.Load( m_xmlFileName );
      m_activeDoc = activeDoc;
      m_errorLogger = new ImportErrorLogger( m_xmlFileName );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="category"></param>
    public void Import( Category category )
    {
      //ElementSet elemSet = Utils.Selection.FilterToCategory( (BuiltInCategory) category.Id.IntegerValue, false, m_activeDoc );

      ICollection<ElementId> ids = Utils.Selection.FilterToCategory( (BuiltInCategory) category.Id.IntegerValue, false, m_activeDoc );

      foreach( ElementId id in ids )
      {
        Element elem = m_activeDoc.GetElement( id );
        Update( elem );
      }
      m_errorLogger.SaveErrorLog();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Import()
    {
      BrowseCategory bcat = new BrowseCategory( m_activeDoc );
      if( bcat.ShowDialog() == DialogResult.OK )
      {
        Category category = bcat.Category;
        //ElementSet elemSet = Utils.Selection.FilterToCategory( (BuiltInCategory) category.Id.IntegerValue, false, m_activeDoc );
        ICollection<ElementId> ids = Utils.Selection.FilterToCategory( (BuiltInCategory) category.Id.IntegerValue, false, m_activeDoc );
        foreach( ElementId id in ids )
        {
          Element elem = m_activeDoc.GetElement( id );
          Update( elem );
        }
        m_errorLogger.SaveErrorLog();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    private void Update( Element elem )
    {
      ArrayList paramsList = GetAllParams( elem );
      XmlNodeList nodeList = m_xmlDoc.DocumentElement.ChildNodes;
      string idAttribute = "Mark";
      XmlAttributeCollection attribsColl = null;
      foreach( Parameter param in paramsList )
      {
        if( param.Definition.Name == idAttribute )
        {
          foreach( XmlNode node in nodeList )
          {
            if( param.AsString() == node.Attributes[idAttribute].Value )
            {
              attribsColl = node.Attributes;
              UpdateParams( paramsList, attribsColl );
              break;
            }
          }
          break;
        }
      }

      if( attribsColl != null )
      {
        UpdateFamily( elem, attribsColl );
      }
    }


    /// <summary>
    /// Tries to update each parameter, if unsuccessfull it logs the error
    /// </summary>
    /// <param name="paramsList">the parameter list of an element</param>
    /// <param name="attribsColl">the parameter list from an attribute collection</param>
    private void UpdateParams( ArrayList paramsList, XmlAttributeCollection attribsColl )
    {
      bool success = true;

      foreach( Parameter param in paramsList )
      {
        try
        {
          string paramDefName = param.Definition.Name.Replace( " ", string.Empty );

          if( attribsColl[paramDefName] != null )
          {

            if( param.AsValueString() != attribsColl[paramDefName].Value )
            {

              if( param.StorageType == StorageType.Double )
              {

                success = param.SetValueString( attribsColl[paramDefName].Value );
                //if (!success) {
                //    success = param.Set(System.Convert.ToDouble(attribsColl[paramDefName].Value));
                //}
              }
              else if( param.StorageType == StorageType.ElementId )
              {
                ElementId elemId = GetElementId( attribsColl[paramDefName].Value );
                if( elemId.IntegerValue != 0 )
                {
                  success = param.Set( elemId );
                }
              }
              else if( param.StorageType == StorageType.Integer )
              {
                success = param.SetValueString( attribsColl[paramDefName].Value );
              }
              else if( param.StorageType == StorageType.String )
              {
                if( param.AsString() != attribsColl[paramDefName].Value )
                {
                  success = param.Set( attribsColl[paramDefName].Value );
                  if( !success )
                  {
                    success = param.SetValueString( attribsColl[paramDefName].Value );
                  }
                }
              }
              else if( param.StorageType == StorageType.None )
              {
                success = param.SetValueString( attribsColl[paramDefName].Value );
              }
            }
          }
        }

            // something wrong happened here
        catch
        {
          success = false;
        }
        // log the error 
        finally
        {
          if( !success )
          {
            m_errorLogger.LogError( param, paramsList );
            success = true;
          }
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="attribsColl"></param>
    private void UpdateFamily( Element elem, XmlAttributeCollection attribsColl )
    {
      string familyName;
      string familySymbolString;
      try
      {
        familyName = attribsColl["Family"].Value;
        familySymbolString = attribsColl["Type"].Value;
      }
      catch( Exception e )
      {
        MessageBox.Show( " The column headers should not be edited", "Import Error" );
        throw e;
      }

      FamilyInstance famInst = elem as FamilyInstance;
      if( famInst != null )
      {

        ElementSet elemSet;
        if( famInst.Symbol.Family.Name != familyName )
        {
          elemSet = GetElementsOfType( typeof( Family ) );

          ElementSetIterator elemSetIter = elemSet.ForwardIterator();
          while( elemSetIter.MoveNext() )
          {
            Family family = elemSetIter.Current as Family;
            if( family.Name == familyName )
            {
              UpdateFamilySymbol( family, famInst, familySymbolString );
              break;
            }
          }
        }
        else
        {
          if( famInst.Symbol.Name != familySymbolString )
          {
            UpdateFamilySymbol( famInst.Symbol.Family, famInst, familySymbolString );
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="family"></param>
    /// <param name="famInst"></param>
    /// <param name="famSymbolString"></param>
    private void UpdateFamilySymbol( Family family, FamilyInstance famInst, string familySymbolString )
    {
      // jeremy migrated from Revit 2014 to 2015:
      //FamilySymbolSet famSymSet = family.Symbols;
      //FamilySymbolSetIterator famSymSetIter = famSymSet.ForwardIterator();
      //while( famSymSetIter.MoveNext() )
      //{
      //  FamilySymbol famSymbol = famSymSetIter.Current as FamilySymbol;

      Document doc = family.Document;

      foreach( ElementId id in family.GetFamilySymbolIds() )
      {
        FamilySymbol famSymbol = doc.GetElement( id ) as FamilySymbol;
        if( famSymbol.Name == familySymbolString )
        {
          famInst.Symbol = famSymbol;
          break;
        }
      }
    }

    /// <summary>
    /// Gets the consolidated list of params for an element, 
    /// ie. its own params + its' family's params 
    /// </summary>
    /// <param name="elem"></param>
    /// <returns></returns>
    private ArrayList GetAllParams( Element elem )
    {
      ArrayList paramsList = new ArrayList();

      // get the parameters of the element
      ParameterSet paramSet = elem.Parameters;
      foreach( Parameter param in paramSet )
      {
        paramsList.Add( param );
      }

      // if elem is a family instance get the parameters of its family too
      Autodesk.Revit.DB.FamilyInstance famInst = null;
      if( elem is Autodesk.Revit.DB.FamilyInstance )
      {
        famInst = elem as Autodesk.Revit.DB.FamilyInstance;
        ParameterSet familyParams = famInst.Symbol.Parameters;
        foreach( Parameter param in familyParams )
        {
          paramsList.Add( param );
        }
      }

      // if elem is a wall type get its parameters too
      Autodesk.Revit.DB.Wall wall = null;
      if( elem is Autodesk.Revit.DB.Wall )
      {
        wall = elem as Autodesk.Revit.DB.Wall;
        ParameterSet wallParams = wall.WallType.Parameters;
        foreach( Parameter param in wallParams )
        {
          paramsList.Add( param );
        }
      }

      return paramsList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elementName"></param>
    /// <returns></returns>
    private ElementId GetElementId( string elementName )
    {
      ElementId elemId = null;
      FilteredElementCollector fec = new FilteredElementCollector( m_activeDoc );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Element ) );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element elem in elements )
      {
        if( elem.Name == elementName )
        {
          elemId = new ElementId( elem.Id.IntegerValue );
          break;
        }
      }

      return elemId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private ElementSet GetElementsOfType( Type type )
    {
      ElementSet elemSet = new ElementSet();
      FilteredElementCollector fec = new FilteredElementCollector( m_activeDoc );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( type );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        elemSet.Insert( element );
      }

      return elemSet;
    }
  }
}
