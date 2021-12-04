using System.Collections;
using System.Collections.Generic;

namespace RevitLookup.Core.RevitTypes
{
    /// <summary>
    ///     Returns the INumeragle as string, separated by semicolon
    /// </summary>
    public class EnumerableAsString : Data
    {
        private readonly IEnumerable _mVal;

        public EnumerableAsString(string label, IEnumerable val) : base(label)
        {
            _mVal = val;
        }

        public override string StrValue()
        {
            if (_mVal == null)
                return "null";

            var stringList = new List<string>();
            foreach (var v in _mVal) stringList.Add(v.ToString());

            return string.Join("; ", stringList);
        }
    }
}