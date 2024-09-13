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

using RevitLookup.Core;

namespace RevitLookup.ViewModels.Dialogs;

public sealed partial class EditParameterViewModel : ObservableObject
{
    [ObservableProperty] private Parameter _parameter;
    
    [ObservableProperty] private string _value;
    [ObservableProperty] private string _parameterName;
    [ObservableProperty] private string _defaultValue;

    public async Task SaveValueAsync()
    {
        await RevitShell.AsyncEventHandler.RaiseAsync(_ =>
        {
            var transaction = new Transaction(Parameter.Element.Document);
            transaction.Start("Set parameter value");

            bool result;
            switch (Parameter.StorageType)
            {
                case StorageType.Integer:
                    result = int.TryParse(Value, out var intValue);
                    if (!result) break;

                    result = Parameter.Set(intValue);
                    break;
                case StorageType.Double:
                    result = Parameter.SetValueString(Value);
                    break;
                case StorageType.String:
                    result = Parameter.Set(Value);
                    break;
                case StorageType.ElementId:
#if REVIT2024_OR_GREATER
                    result = long.TryParse(Value, out var idValue);
#else
                    result = int.TryParse(Value, out var idValue);
#endif
                    if (!result) break;

                    result = Parameter.Set(new ElementId(idValue));
                    break;
                case StorageType.None:
                default:
                    result = Parameter.SetValueString(Value);
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

    partial void OnParameterChanged(Parameter value)
    {
        Value = value.StorageType switch
        {
            StorageType.Integer => value.AsInteger().ToString(),
            StorageType.Double => value.AsValueString(),
            StorageType.String => value.AsString(),
            StorageType.ElementId => value.AsElementId().ToString(),
            StorageType.None => value.AsValueString(),
            _ => value.AsValueString()
        };

        DefaultValue = Value;
        ParameterName = value.Definition.Name;
    }
}