using Autodesk.Revit.DB;

namespace RevitLookup.Core.RevitTypes.PlaceHolders;

internal interface IObjectToSnoopPlaceholder
{
    object GetObject(Document document);
    string GetName();
    Type GetUnderlyingType();
}