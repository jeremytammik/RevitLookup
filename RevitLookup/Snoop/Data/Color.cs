namespace RevitLookup.Snoop.Data
{
    public class Color : Data
    {
        private Autodesk.Revit.DB.Color _mColor;

        public Color(string label, Autodesk.Revit.DB.Color color) : base(label)
        {
            _mColor = color;
        }

        public override string StrValue()
        {
            return _mColor.IsValid
                ? $"R: {_mColor.Red}; G: {_mColor.Green}; B: {_mColor.Blue}"
                : "-- invalid color value --";
        }
    }
}