using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
  public class ExtensibleStorageEntityContentStream : IElementStream
  {
    private readonly Document _document;
    private readonly ArrayList _data;
    private readonly Entity _entity;

    public ExtensibleStorageEntityContentStream( Document document, ArrayList data, object elem )
    {
      _document = document;
      _data = data;
      _entity = elem as Entity;
    }

    public void Stream( Type type )
    {
      if( type != typeof( Entity ) || _entity == null || !_entity.IsValid() )
        return;

      if( !_entity.ReadAccessGranted() )
        _data.Add( new Snoop.Data.Exception(
          "<Extensible storage Fields>", new Exception(
            "Doesn't have access to read extensible storage data" ) ) );

      var fields = _entity.Schema.ListFields();

      if( !fields.Any() )
        return;

      _data.Add( new Data.ExtensibleStorageSeparator() );

      foreach( var field in fields )
        StreamEntityFieldValue( field );
    }

    private void StreamEntityFieldValue(Field field)
    {
        try
        {
            var getEntityValueMethod = GetEntityFieldValueMethod(field);

            var valueType = GetFieldValueType(field);

            var genericGet = getEntityValueMethod.MakeGenericMethod(valueType);

            var fieldSpecType = field.GetSpecTypeId();

            var unit = UnitUtils.IsMeasurableSpec(fieldSpecType) ? UnitUtils.GetValidUnits(field.GetSpecTypeId()).First() : UnitTypeId.Custom;

            var parameters = getEntityValueMethod.GetParameters().Length == 1
                ? new object[] {field}
                : new object[] {field, unit};

            var value = genericGet.Invoke(_entity, parameters);

            AddFieldValue(field, value);
        }
        catch (Exception ex)
        {
            _data.Add(new Data.Exception(field.FieldName, ex));
        }
    }

    private Type GetFieldValueType( Field field )
    {
      switch( field.ContainerType )
      {
        case ContainerType.Simple:
          return field.ValueType;

        case ContainerType.Array:
          var generic = typeof( IList<> );

          return generic.MakeGenericType( field.ValueType );

        case ContainerType.Map:
          var genericMap = typeof( IDictionary<,> );

          return genericMap.MakeGenericType( field.KeyType, field.ValueType );

        default:
          throw new NotSupportedException();
      }
    }

    private void AddFieldValue( Field field, object value )
    {
      try
      {
        if( field.ContainerType != ContainerType.Simple )
          _data.Add( new Snoop.Data.Enumerable( field.FieldName, value as IEnumerable ) );
        else if( field.ValueType == typeof( double ) )
          _data.Add( new Snoop.Data.Double( field.FieldName, (double) value ) );
        else if( field.ValueType == typeof( string ) )
          _data.Add( new Snoop.Data.String( field.FieldName, value as string ) );
        else if( field.ValueType == typeof( XYZ ) )
          _data.Add( new Data.Xyz( field.FieldName, value as XYZ ) );
        else if( field.ValueType == typeof( UV ) )
          _data.Add( new Data.Uv( field.FieldName, value as UV ) );
        else if( field.ValueType == typeof( int ) )
          _data.Add( new Data.Int( field.FieldName, (int) value ) );
        else if( field.ValueType == typeof( ElementId ) )
          _data.Add( new Snoop.Data.ElementId( field.FieldName, value as ElementId, _document ) );
        else if( field.ValueType == typeof( Guid ) )
        {
          var guidValue = (Guid) value;

          _data.Add( new Snoop.Data.String( field.FieldName, guidValue.ToString() ) );
        }
        else
          _data.Add( new Snoop.Data.Object( field.FieldName, value ) );
      }
      catch( Exception ex )
      {
        _data.Add( new Snoop.Data.Exception( field.FieldName, ex ) );
      }
    }

    private static MethodInfo GetEntityFieldValueMethod(Field field)
    {
        return typeof(Entity)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.Name == nameof(Entity.Get) && x.IsGenericMethod)
            .Single(x => IsGetByFieldMethod(x, field));
    }

    private static bool IsGetByFieldMethod(MethodInfo methodInfo, Field field)
    {
        var parameters = methodInfo.GetParameters();
        
        var fieldSpecType = field.GetSpecTypeId();

        if (UnitUtils.IsMeasurableSpec(fieldSpecType) || fieldSpecType == SpecTypeId.Custom)
        {
            var firstParameter = parameters.First();

            var lastParameter = parameters.Last();

            return parameters.Length == 2 && firstParameter.ParameterType == typeof(Field) && lastParameter.ParameterType == typeof(ForgeTypeId);
        }

        return parameters.Length == 1 && parameters.Single().ParameterType == typeof(Field);
    }
  }
}
