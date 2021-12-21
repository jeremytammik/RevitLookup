namespace RevitLookup.Core.RevitTypes;

public class ExtensibleStorageSeparator : Data
{
    public ExtensibleStorageSeparator() : base(string.Empty)
    {
    }

    public override bool IsSeparator => true;

    public override bool HasDrillDown => false;

    public override string AsValueString()
    {
        return "--- Extensible storages ---";
    }
}