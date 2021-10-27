
using System;

namespace RevitLookup.Snoop.Data
{
    /// <summary>
    /// Snoop.Data class to hold and format a ClassSeparator value.
    /// </summary>

    public class MemberSeparator : Data
    {
        protected string Name;

        public MemberSeparator(string val)
        : base("------- CLASS -------")
        {
            Name = val;
        }

        override public string
        StrValue()
        {
            return $"--- {Name} ---";
        }

        public override bool
        IsSeparator =>
            true;

        public override bool
        HasDrillDown =>
            false;

        public override System.Windows.Forms.Form DrillDown()
        {
            return null;
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
            return $"  --- {Name} ---";
        }
    }
}
