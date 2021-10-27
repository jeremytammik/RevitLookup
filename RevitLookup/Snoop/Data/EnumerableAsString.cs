using System.Collections;
using System.Collections.Generic;

namespace RevitLookup.Snoop.Data
{
    /// <summary>
    /// Returns the INumeragle as string, separated by semicolon
    /// </summary>
    public class EnumerableAsString : Data
    {
        private readonly IEnumerable m_val;

        public EnumerableAsString(string label, IEnumerable val) : base(label)
        {
            m_val = val;
        }

        public override string StrValue()
        {
            if (m_val == null)
                return "null";

            List<string> stringList = new List<string>();
            foreach (var v in m_val)
            {
                stringList.Add(v.ToString());
            }

            return string.Join("; ", stringList);
        }
    }
}