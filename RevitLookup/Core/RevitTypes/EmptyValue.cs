namespace RevitLookup.Core.RevitTypes;

public class EmptyValue : Data
{
    public EmptyValue(string label) : base(label)
    {
    }

    public override string StrValue()
    {
        return string.Empty;
    }
}