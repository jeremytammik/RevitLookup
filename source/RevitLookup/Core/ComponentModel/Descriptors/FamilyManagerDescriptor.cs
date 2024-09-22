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

using System.Reflection;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class FamilyManagerDescriptor(FamilyManager familyManager) : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyManager.GetAssociatedFamilyParameter) => ResolveGetAssociatedFamilyParameter,
            nameof(FamilyManager.IsParameterLockable) => ResolveIsParameterLockable,
            nameof(FamilyManager.IsParameterLocked) => ResolveIsParameterLocked,
            _ => null
        };

        IVariants ResolveGetAssociatedFamilyParameter()
        {
            var elementTypes = Context.ActiveDocument.GetElements().WhereElementIsElementType();
            var elementInstances = Context.ActiveDocument.GetElements().WhereElementIsNotElementType();
            var elements = elementTypes
                .UnionWith(elementInstances)
                .ToElements();

            var resolveSet = new Variants<KeyValuePair<Parameter, FamilyParameter>>(elements.Count);
            foreach (var element in elements)
            {
                foreach (Parameter parameter in element.Parameters)
                {
                    var familyParameter = familyManager.GetAssociatedFamilyParameter(parameter);
                    if (familyParameter is not null)
                    {
                        resolveSet.Add(new KeyValuePair<Parameter, FamilyParameter>(parameter, familyParameter));
                    }
                }
            }

            return resolveSet;
        }

        IVariants ResolveIsParameterLockable()
        {
            var familyParameters = familyManager.Parameters;
            var resolveSet = new Variants<bool>(familyParameters.Size);
            foreach (FamilyParameter parameter in familyParameters)
            {
                var result = familyManager.IsParameterLockable(parameter);
                resolveSet.Add(result, $"{parameter.Definition.Name}: {result}");
            }

            return resolveSet;
        }

        IVariants ResolveIsParameterLocked()
        {
            var familyParameters = familyManager.Parameters;
            var resolveSet = new Variants<bool>(familyParameters.Size);
            foreach (FamilyParameter parameter in familyParameters)
            {
                var result = familyManager.IsParameterLocked(parameter);
                resolveSet.Add(result, $"{parameter.Definition.Name}: {result}");
            }

            return resolveSet;
        }
    }
}