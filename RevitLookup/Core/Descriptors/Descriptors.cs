﻿// Copyright 2003-2022 by Autodesk, Inc.
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

namespace RevitLookup.Core.Descriptors;

public sealed class BoolDescriptor : Descriptor
{
    public BoolDescriptor(bool value)
    {
        Label = value ? "True" : "False";
    }
}

public sealed class StringDescriptor : Descriptor
{
    public StringDescriptor(string value)
    {
        Label = value;
    }
}

public sealed class IntDescriptor : Descriptor
{
    public IntDescriptor(int value)
    {
        Label = value.ToString();
    }
}

public sealed class ObjectDescriptor : Descriptor
{
    public ObjectDescriptor(object value)
    {
        Label = $"Unsupported type: {value}";
    }
}