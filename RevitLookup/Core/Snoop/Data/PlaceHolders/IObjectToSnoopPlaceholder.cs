using System;
using Autodesk.Revit.DB;

namespace RevitLookup.Core.Snoop.Data.PlaceHolders
{
    internal interface IObjectToSnoopPlaceholder
    {
        object GetObject(Document document);
        string GetName();
        Type GetUnderlyingType();
    }
}