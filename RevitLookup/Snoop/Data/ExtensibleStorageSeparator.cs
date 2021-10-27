namespace RevitLookup.Snoop.Data
{
    public class ExtensibleStorageSeparator : Data
    {
        public ExtensibleStorageSeparator() : base(string.Empty)
        {
        }

        public override string StrValue()
        {
            return "--- Extensible storages ---";
        }

        public override bool IsSeparator
        {
            get { return true; }
        }

        public override bool HasDrillDown
        {
            get { return false; }
        }
    }
}