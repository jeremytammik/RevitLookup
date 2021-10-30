using Autodesk.Revit.DB;

namespace RevitLookup.Snoop
{
    internal interface IHaveCollector
    {
        void SetDocument(Document document);
    }
}