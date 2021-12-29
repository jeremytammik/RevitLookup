namespace RevitLookup.Core.RevitTypes;

public class ExtensibleStorageSeparatorData : Data
{
    public ExtensibleStorageSeparatorData() : base(string.Empty)
    {
    }

    public override bool IsSeparator => true;

    public override bool HasDrillDown => false;

    public override string AsValueString()
    {
        return "--- Extensible storages ---";
    }
}