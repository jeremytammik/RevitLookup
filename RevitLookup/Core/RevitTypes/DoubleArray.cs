namespace RevitLookup.Core.RevitTypes
{
    public class DoubleArray : Data
    {
        private readonly Autodesk.Revit.DB.DoubleArray _value;

        public DoubleArray(string label, Autodesk.Revit.DB.DoubleArray val) : base(label)
        {
            _value = val;
        }

        public override string StrValue()
        {
            var array = new string[_value.Size];
            for (var i = 0; i < _value.Size; i++) array[i] = _value.get_Item(i).ToString();
            return string.Join("; ", array);
        }
    }
}