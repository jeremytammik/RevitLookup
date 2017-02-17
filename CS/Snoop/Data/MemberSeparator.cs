
using System;

namespace RevitLookup.Snoop.Data
{
    /// <summary>
    /// Snoop.Data class to hold and format a ClassSeparator value.
    /// </summary>

    public class MemberSeparator : Data
    {
        protected string name;

        public MemberSeparator(string val)
        : base("------- CLASS -------")
        {
            name = val;
        }

        override public string
        StrValue()
        {
            return string.Format("--- {0} ---", name);
        }

        public override bool
        IsSeparator
        {
            get { return true; }
        }

        public override bool
        HasDrillDown
        {
            get { return false; }
        }

        public override void
        DrillDown()
        {

        }
    }

    public class MemberSeparatorWithOffset : MemberSeparator
    {
        public MemberSeparatorWithOffset(string val)
            : base(val)
        {
        }

        override public string StrValue()
        {
            return string.Format("  --- {0} ---", name);
        }
    }
}
