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

namespace RevitLookup.Core.Objects;

public sealed class ResolveSummary
{
    public string Description { get; private set; }
    public object Result { get; private set; }
    public List<ResolveSummary> Variants { get; private set; }

    public static ResolveSummary Append(object result)
    {
        return new ResolveSummary
        {
            Result = result
        };
    }

    public static ResolveSummary Append(object result, string description)
    {
        return new ResolveSummary
        {
            Description = description,
            Result = result
        };
    }

    public ResolveSummary AppendVariant(object result)
    {
        if (result is null) return this;
        if (Result is null)
        {
            Result = result;
            return this;
        }

        Variants ??= new List<ResolveSummary>(2) {this};
        Variants.Add(Append(result));
        return this;
    }

    public ResolveSummary AppendVariant(object result, string description)
    {
        if (result is null) return this;
        if (Result is null)
        {
            Description = description;
            Result = result;
            return this;
        }

        Variants ??= new List<ResolveSummary>(2) {this};
        Variants.Add(Append(result, description));
        return this;
    }
}