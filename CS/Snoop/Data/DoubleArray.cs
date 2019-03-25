namespace RevitLookup.Snoop.Data
{
    public class DoubleArray : Data
    {
        protected Autodesk.Revit.DB.DoubleArray m_val;

        public DoubleArray(string label, Autodesk.Revit.DB.DoubleArray val) : 
            base(label)
        {
            m_val = val;
        }

        public override string StrValue()
        {
            string[] array = new string[m_val.Size];

            for (int i = 0; i < m_val.Size; i++)
            {
                array[i] = m_val.get_Item(i).ToString();
            }

            return string.Join("; ", array);
        }
    }
}