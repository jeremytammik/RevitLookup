using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes.PlaceHolders;

internal interface ISnoopPlaceholder
{
    object GetObject(Document document);
    string GetName();
    Type GetUnderlyingType();
}