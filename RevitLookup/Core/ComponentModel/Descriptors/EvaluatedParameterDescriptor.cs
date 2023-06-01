// Copyright 2003-2023 by Autodesk, Inc.
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

#if R24_OR_GREATER
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class EvaluatedParameterDescriptor : Descriptor, IDescriptorResolver
{
    private readonly EvaluatedParameter _parameter;

    public EvaluatedParameterDescriptor(EvaluatedParameter parameter)
    {
        _parameter = parameter;
        Name = parameter.Definition.Name;
    }

    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(EvaluatedParameter.AsValueString) when parameters.Length == 1 => ResolveSet.Append(_parameter.AsValueString(context)),
            nameof(EvaluatedParameter.AsValueString) when parameters.Length == 2 => ResolveAsValueStringFormat(),
            _ => null
        };

        ResolveSet ResolveAsValueStringFormat()
        {
            var dataType = _parameter.Definition.GetDataType();
            var options = UnitUtils.IsMeasurableSpec(dataType) ? context.GetUnits().GetFormatOptions(dataType) : new FormatOptions();
            return ResolveSet.Append(_parameter.AsValueString(context, options));
        }
    }
}
#endif