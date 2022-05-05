using System.Collections;

namespace RevitLookup.Core.RevitTypes;

/// <summary>
///     Returns the IEnumerable as string, separated by semicolon
/// </summary>
public class EnumerableAsString : Data
{
    private readonly IEnumerable _value;

    public EnumerableAsString(string label, IEnumerable val) : base(label)
    {
        _value = val;
    }

    public override string Value
    {
        get
        {
            if (_value is null) return Labels.Null;

            var stringList = _value
                .Cast<object>()
                .Select(v => v.ToString());
            return string.Join("; ", stringList);
        }
    }
}