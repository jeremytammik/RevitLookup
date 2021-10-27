using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
  public class FamilyTypeParameterValuesStream : IElementStream
  {
    private readonly ArrayList _data;
    private readonly object _elem;

    public FamilyTypeParameterValuesStream( ArrayList data, object elem )
    {
      _data = data;
      _elem = elem;
    }

    public void Stream( Type type )
    {
      if( type != typeof( Parameter ) )
        return;

      var parameter = (Parameter) _elem;

      var family = (parameter.Element as FamilyInstance)?.Symbol.Family 
        ?? (parameter.Element as FamilySymbol)?.Family;

      // Filter out non family types.

      //if (parameter.Definition.ParameterType != ParameterType.FamilyType || family == null) // Revit 2021
      //  return;

      if (!Category.IsBuiltInCategory(parameter.Definition.GetDataType())) // Revit 2022
        return;

      var familyTypeParameterValues = family
          .GetFamilyTypeParameterValues( parameter.Id )
          .Select( family.Document.GetElement )
          .ToList();

      _data.Add( new Data.Enumerable( 
        $"{nameof( Family )}.{nameof( Family.GetFamilyTypeParameterValues )}()", 
        familyTypeParameterValues ) );
    }
  }
}