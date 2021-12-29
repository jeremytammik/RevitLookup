namespace RevitLookup.Core.RevitTypes;

public class MemberSeparatorData : Data
{
    protected readonly string Name;

    protected MemberSeparatorData(string val) : base("------- CLASS -------")
    {
        Name = val;
    }

    public override bool IsSeparator => true;

    public override bool HasDrillDown => false;

    public override string AsValueString()
    {
        return $"--- {Name} ---";
    }

    public override Form DrillDown()
    {
        return null;
    }
}

public class MemberSeparatorWithOffsetData : MemberSeparatorData
{
    public MemberSeparatorWithOffsetData(string val) : base(val)
    {
    }

    public override string AsValueString()
    {
        return $"  --- {Name} ---";
    }
}