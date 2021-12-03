namespace RevitLookup.Core.Data
{
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
}