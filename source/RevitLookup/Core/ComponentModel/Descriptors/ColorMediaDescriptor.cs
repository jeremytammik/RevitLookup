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

using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using RevitLookup.Utils;
using Color = System.Windows.Media.Color;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ColorMediaDescriptor : Descriptor, IDescriptorExtension
{
    private readonly Color _color;
    
    public ColorMediaDescriptor(Color color)
    {
        _color = color;
        Name = $"RGB: {color.R} {color.B} {color.B}";
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("HEX", _ => ColorRepresentationUtils.ColorToHex(_color.GetDrawingColor()));
        manager.Register("HEX int", _ => ColorRepresentationUtils.ColorToHexInteger(_color.GetDrawingColor()));
        manager.Register("RGB", _ => ColorRepresentationUtils.ColorToRgb(_color.GetDrawingColor()));
        manager.Register("HSL", _ => ColorRepresentationUtils.ColorToHsl(_color.GetDrawingColor()));
        manager.Register("HSV", _ => ColorRepresentationUtils.ColorToHsv(_color.GetDrawingColor()));
        manager.Register("CMYK", _ => ColorRepresentationUtils.ColorToCmyk(_color.GetDrawingColor()));
        manager.Register("HSB", _ => ColorRepresentationUtils.ColorToHsb(_color.GetDrawingColor()));
        manager.Register("HSI", _ => ColorRepresentationUtils.ColorToHsi(_color.GetDrawingColor()));
        manager.Register("HWB", _ => ColorRepresentationUtils.ColorToHwb(_color.GetDrawingColor()));
        manager.Register("NCol", _ => ColorRepresentationUtils.ColorToNCol(_color.GetDrawingColor()));
        manager.Register("CIELAB", _ => ColorRepresentationUtils.ColorToCielab(_color.GetDrawingColor()));
        manager.Register("CIEXYZ", _ => ColorRepresentationUtils.ColorToCieXyz(_color.GetDrawingColor()));
        manager.Register("VEC4", _ => ColorRepresentationUtils.ColorToFloat(_color.GetDrawingColor()));
        manager.Register("Decimal", _ => ColorRepresentationUtils.ColorToDecimal(_color.GetDrawingColor()));
        manager.Register("Name", _ => ColorRepresentationUtils.GetColorName(_color.GetDrawingColor()));
    }
}