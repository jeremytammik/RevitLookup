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

using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Utils;
using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ColorDescriptor : Descriptor, IDescriptorExtension
{
    private readonly Color _color;

    public ColorDescriptor(Color color)
    {
        _color = color;
        Name = color.IsValid ? $"RGB: {color.Red} {color.Green} {color.Blue}" : "The color represents uninitialized/invalid value";
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(_color, extension =>
        {
            extension.Name = "HEX";
            extension.Result = ColorRepresentationUtils.ColorToHex(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HEX int";
            extension.Result = ColorRepresentationUtils.ColorToHexInteger(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "RGB";
            extension.Result = ColorRepresentationUtils.ColorToRgb(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HSL";
            extension.Result = ColorRepresentationUtils.ColorToHsl(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HSV";
            extension.Result = ColorRepresentationUtils.ColorToHsv(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "CMYK";
            extension.Result = ColorRepresentationUtils.ColorToCmyk(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HSB";
            extension.Result = ColorRepresentationUtils.ColorToHsb(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HSI";
            extension.Result = ColorRepresentationUtils.ColorToHsi(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "HWB";
            extension.Result = ColorRepresentationUtils.ColorToHwb(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "NCol";
            extension.Result = ColorRepresentationUtils.ColorToNCol(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "CIELAB";
            extension.Result = ColorRepresentationUtils.ColorToCielab(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "CIEXYZ";
            extension.Result = ColorRepresentationUtils.ColorToCieXyz(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "VEC4";
            extension.Result = ColorRepresentationUtils.ColorToFloat(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "Decimal";
            extension.Result = ColorRepresentationUtils.ColorToDecimal(extension.Value.GetMediaColor());
        });
        manager.Register(_color, extension =>
        {
            extension.Name = "Name";
            extension.Result = ColorRepresentationUtils.GetColorName(extension.Value.GetMediaColor());
        });
    }
}