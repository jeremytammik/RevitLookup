namespace RevitLookup.Snoop.Data
{
    public class Color : Data
    {
        private Autodesk.Revit.DB.Color m_color;

        public Color(string label, Autodesk.Revit.DB.Color color) : base(label)
        {
            m_color = color;
        }

        public override string StrValue()
        {
            return m_color.IsValid
                ? $"R: {m_color.Red}; G: {m_color.Green}; B: {m_color.Blue}"
                : "-- invalid color value --";
        }
    }
}