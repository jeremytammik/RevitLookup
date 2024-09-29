// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Collections.ObjectModel;
using System.Reflection;
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitLookup.Core;

namespace RevitLookup.ViewModels.Dialogs.ExtensibleStorage;

public sealed partial class EditEntityViewModel : ObservableObject
{
    private readonly Entity _entity;
    private readonly Element _element;
    [ObservableProperty] private ObservableCollection<SimpleFieldDto> _descriptors = [];
    
    public EditEntityViewModel(Entity entity, Element element)
    {
        _entity = entity;
        _element = element;
        var fields = entity.Schema.ListFields();
        foreach (var field in fields)
        {
            if (field.ContainerType == ContainerType.Simple)
            {
                var method = entity.GetType().GetMethod(nameof(Entity.Get), [typeof(Field)])!;
                var genericMethod = MakeGenericInvoker(field, method);
                var value = genericMethod.Invoke(entity, [field]);
                
                Descriptors.Add(new SimpleFieldDto
                {
                    FieldType = field.ValueType,
                    Name = field.FieldName,
                    Value = value,
                    Unit = field.GetSpecTypeId()
                });
            }
        }
    }
    
    private static MethodInfo MakeGenericInvoker(Field field, MethodInfo invoker)
    {
        var containerType = field.ContainerType switch
        {
            ContainerType.Simple => field.ValueType,
            ContainerType.Array => typeof(IList<>).MakeGenericType(field.ValueType),
            ContainerType.Map => typeof(IDictionary<,>).MakeGenericType(field.KeyType, field.ValueType),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return invoker.MakeGenericMethod(containerType);
    }
    
    public async Task SaveValueAsync()
    {
        await RevitShell.AsyncEventHandler.RaiseAsync(_ =>
        {
            var transaction = new Transaction(_element.Document);
            transaction.Start("Edit entity values");
            
            foreach (var descriptor in Descriptors)
            {
                var type = descriptor.FieldType;
                if (type == typeof(string))
                {
                    _entity.Set(descriptor.Name, descriptor.Value.ToString());
                }
                else if (type == typeof(double))
                {
                    if (double.TryParse(descriptor.Value.ToString(), out var value))
                    {
                        _entity.Set(descriptor.Name, value);
                    }
                }
                else if (type == typeof(int))
                {
                    if (int.TryParse(descriptor.Value.ToString(), out var value))
                    {
                        _entity.Set(descriptor.Name, value);
                    }
                }
                else if (type == typeof(short))
                {
                    if (short.TryParse(descriptor.Value.ToString(), out var value))
                    {
                        _entity.Set(descriptor.Name, value);
                    }
                }
                _element.SetEntity(_entity);
            }
            
            transaction.Commit();
        });
    }
}