namespace RevitLookup.Snoop.Data
{
    public class DoubleArray : Data
    {
        protected Autodesk.Revit.DB.DoubleArray MVal;

        public DoubleArray(string label, Autodesk.Revit.DB.DoubleArray val) : 
            base(label)
        {
            MVal = val;
        }

        public override string StrValue()
        {
            string[] array = new string[MVal.Size];

            for (int i = 0; i < MVal.Size; i++)
            {
                array[i] = MVal.get_Item(i).ToString();
            }

            return string.Join("; ", array);
        }
    }
}