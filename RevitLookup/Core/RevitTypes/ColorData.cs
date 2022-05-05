using Color = Autodesk.Revit.DB.Color;

namespace RevitLookup.Core.RevitTypes;

public class ColorData : Data
{
    private readonly Color _color;

    public ColorData(string label, Color color) : base(label)
    {
        _color = color;
    }

    public override string Value => _color.IsValid
        ? $"R: {_color.Red}; G: {_color.Green}; B: {_color.Blue}"
        : "-- invalid color value --";
}