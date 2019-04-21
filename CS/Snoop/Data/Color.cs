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
            return string.Format("R: {0}; G: {1}; B: {2}", m_color.Red, m_color.Green, m_color.Blue);
        }
    }
}