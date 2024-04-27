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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class TableDataDescriptor(TableData tableData) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(TableData.GetSectionData) => ResolveSectionData(),
            nameof(TableData.IsValidZoomLevel) => ResolveZoomLevel(),
            _ => null
        };
        
        ResolveSet ResolveSectionData()
        {
            var values = Enum.GetValues(typeof(SectionType));
            var resolveSummary = new ResolveSet(values.Length);
            foreach (SectionType type in values)
            {
                resolveSummary.AppendVariant(tableData.GetSectionData(type), type.ToString());
            }
            
            return resolveSummary;
        }
        
        ResolveSet ResolveZoomLevel()
        {
            var resolveSummary = new ResolveSet();
            for (var i = 1; i < 511; i += 1)
            {
                var result = tableData.IsValidZoomLevel(i);
                if (result)
                    resolveSummary.AppendVariant(true, $"{i}: True");
            }
            
            return resolveSummary;
        }
    }
}