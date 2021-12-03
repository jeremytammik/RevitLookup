namespace RevitLookup.Core.Data
{
    public class Color : Data
    {
        private readonly Autodesk.Revit.DB.Color _color;

        public Color(string label, Autodesk.Revit.DB.Color color) : base(label)
        {
            _color = color;
        }

        public override string StrValue()
        {
            return _color.IsValid
                ? $"R: {_color.Red}; G: {_color.Green}; B: {_color.Blue}"
                : "-- invalid color value --";
        }
    }
}