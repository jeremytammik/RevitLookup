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
using Autodesk.Revit.DB.Lighting;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class LightFamilyDescriptor (LightFamily lightFamily) : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(LightFamily.GetLightTypeName) => ResolveLightTypeName,
            nameof(LightFamily.GetLightType) => ResolveLightType,
            _ => null
        };
        
        IVariants ResolveLightTypeName()
        {
            var capacity = lightFamily.GetNumberOfLightTypes();
            var variants = new Variants<string>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var name = lightFamily.GetLightTypeName(i);
                variants.Add(name);
            }
            
            return variants;
        }
        IVariants ResolveLightType()
        {
            var capacity = lightFamily.GetNumberOfLightTypes();
            var variants = new Variants<LightType>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var type = lightFamily.GetLightType(i);
                variants.Add(type, $"Index {i}");
            }
            
            return variants;
        }
    }
}