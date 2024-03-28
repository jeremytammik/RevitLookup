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

using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitLookup.ViewModels.Dialogs;

public partial class EditParameterViewModel : ObservableObject
{
    private readonly Parameter _parameter;
    
    [ObservableProperty] private string _value;
    
    public EditParameterViewModel(Parameter parameter)
    {
        _parameter = parameter;
        _value = parameter.StorageType switch
        {
            StorageType.Integer => parameter.AsInteger().ToString(),
            StorageType.Double => parameter.AsValueString(),
            StorageType.String => parameter.AsString(),
            StorageType.ElementId => parameter.AsElementId().ToString(),
            StorageType.None => parameter.AsValueString(),
            _ => parameter.AsValueString()
        };
        
        DefaultValue = _value;
        ParameterName = parameter.Definition.Name;
    }
    
    public string ParameterName { get; }
    public string DefaultValue { get; }
    
    public async Task SaveValueAsync()
    {
        await Application.AsyncEventHandler.RaiseAsync(_ =>
        {
            var transaction = new Transaction(_parameter.Element.Document);
            transaction.Start("Set parameter value");
            
            bool result;
            switch (_parameter.StorageType)
            {
                case StorageType.Integer:
                    result = int.TryParse(Value, out var intValue);
                    if (!result) break;
                    
                    result = _parameter.Set(intValue);
                    break;
                case StorageType.Double:
                    result = _parameter.SetValueString(Value);
                    break;
                case StorageType.String:
                    result = _parameter.Set(Value);
                    break;
                case StorageType.ElementId:
#if R24_OR_GREATER
                    result = long.TryParse(Value, out var idValue);
#else
                    result = int.TryParse(Value, out var idValue);
#endif
                    if (!result) break;
                    
                    result = _parameter.Set(new ElementId(idValue));
                    break;
                case StorageType.None:
                default:
                    result = _parameter.SetValueString(Value);
                    break;
            }
            
            if (result)
            {
                transaction.Commit();
            }
            else
            {
                transaction.RollBack();
                throw new ArgumentException("Invalid parameter value");
            }
        });
    }
}