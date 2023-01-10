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

using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Extensions;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ParameterDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly Parameter _parameter;

    public ParameterDescriptor(Parameter parameter)
    {
        _parameter = parameter;
        Label = parameter.Definition.Name;
    }

    public void RegisterExtensions(ExtensionManager manager)
    {
        manager.Register(new DescriptorExtension<Parameter>(_parameter)
        {
            Name = nameof(ParameterExtensions.AsBool),
            Value = parameter => parameter.AsBool()
        });

        manager.Register(new DescriptorExtension<Parameter>(_parameter)
        {
            Name = nameof(ParameterExtensions.AsColor),
            Value = parameter => parameter.AsColor()
        });
    }

    public void RegisterResolvers(IResolverManager manager)
    {
        manager.Register(nameof(Parameter.ClearValue), false);
    }
}