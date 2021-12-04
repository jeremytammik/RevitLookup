using System.Collections;
using System.Linq;

namespace RevitLookup.Core.RevitTypes
{
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

        public override string StrValue()
        {
            if (_value == null) return "null";

            var stringList = _value
                .Cast<object>()
                .Select(v => v.ToString());
            return string.Join("; ", stringList);
        }
    }
}