using System.Windows.Forms;

namespace RevitLookup.Core.RevitTypes
{
    /// <summary>
    ///     Snoop.Data class to hold and format a ClassSeparator value.
    /// </summary>
    public class MemberSeparator : Data
    {
        protected readonly string Name;

        protected MemberSeparator(string val) : base("------- CLASS -------")
        {
            Name = val;
        }

        public override bool IsSeparator => true;

        public override bool HasDrillDown => false;

        public override string StrValue()
        {
            return $"--- {Name} ---";
        }

        public override Form DrillDown()
        {
            return null;
        }
    }

    public class MemberSeparatorWithOffset : MemberSeparator
    {
        public MemberSeparatorWithOffset(string val) : base(val)
        {
        }

        public override string StrValue()
        {
            return $"  --- {Name} ---";
        }
    }
}