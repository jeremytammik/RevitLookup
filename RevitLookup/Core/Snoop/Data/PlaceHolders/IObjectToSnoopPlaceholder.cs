using System;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data.PlaceHolders
{
    internal interface IObjectToSnoopPlaceholder
    {
        object GetObject(Document document);
        string GetName();
        Type GetUnderlyingType();
    }
}