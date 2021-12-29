using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes;

public class DoubleArrayData : Data
{
    private readonly DoubleArray _value;

    public DoubleArrayData(string label, DoubleArray val) : base(label)
    {
        _value = val;
    }

    public override string AsValueString()
    {
        var array = new string[_value.Size];
        for (var i = 0; i < _value.Size; i++) array[i] = _value.get_Item(i).ToString();
        return string.Join("; ", array);
    }
}