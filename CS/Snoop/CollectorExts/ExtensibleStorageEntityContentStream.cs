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
    private readonly Document document;
    private readonly ArrayList data;
    private readonly Entity entity;

    public ExtensibleStorageEntityContentStream( Document document, ArrayList data, object elem )
    {
      this.document = document;
      this.data = data;
      entity = elem as Entity;
    }

    public void Stream( Type type )
    {
      if( type != typeof( Entity ) || entity == null || !entity.IsValid() )
        return;

      if( !entity.ReadAccessGranted() )
        data.Add( new Snoop.Data.Exception(
          "<Extensible storage Fields>", new Exception(
            "Doesn't have access to read extensible storage data" ) ) );

      var fields = entity.Schema.ListFields();

      if( !fields.Any() )
        return;

      data.Add( new Snoop.Data.ExtensibleStorageSeparator() );

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

            var value = genericGet.Invoke(entity, parameters);

            AddFieldValue(field, value);
        }
        catch (Exception ex)
        {
            data.Add(new Data.Exception(field.FieldName, ex));
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
          data.Add( new Snoop.Data.Enumerable( field.FieldName, value as IEnumerable ) );
        else if( field.ValueType == typeof( double ) )
          data.Add( new Snoop.Data.Double( field.FieldName, (double) value ) );
        else if( field.ValueType == typeof( string ) )
          data.Add( new Snoop.Data.String( field.FieldName, value as string ) );
        else if( field.ValueType == typeof( XYZ ) )
          data.Add( new Snoop.Data.Xyz( field.FieldName, value as XYZ ) );
        else if( field.ValueType == typeof( UV ) )
          data.Add( new Snoop.Data.Uv( field.FieldName, value as UV ) );
        else if( field.ValueType == typeof( int ) )
          data.Add( new Snoop.Data.Int( field.FieldName, (int) value ) );
        else if( field.ValueType == typeof( ElementId ) )
          data.Add( new Snoop.Data.ElementId( field.FieldName, value as ElementId, document ) );
        else if( field.ValueType == typeof( Guid ) )
        {
          var guidValue = (Guid) value;

          data.Add( new Snoop.Data.String( field.FieldName, guidValue.ToString() ) );
        }
        else
          data.Add( new Snoop.Data.Object( field.FieldName, value ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( field.FieldName, ex ) );
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
