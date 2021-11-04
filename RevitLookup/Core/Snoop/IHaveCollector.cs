using Autodesk.Revit.DB;

namespace RevitLookup.Core.Snoop
{
    internal interface IHaveCollector
    {
        void SetDocument(Document document);
    }
}