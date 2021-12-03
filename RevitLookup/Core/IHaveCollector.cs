using Autodesk.Revit.DB;

namespace RevitLookup.Core
{
    internal interface IHaveCollector
    {
        void SetDocument(Document document);
    }
}