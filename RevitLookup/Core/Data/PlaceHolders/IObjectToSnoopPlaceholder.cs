using System;
using Autodesk.Revit.DB;

namespace RevitLookup.Core.Data.PlaceHolders
{
    internal interface IObjectToSnoopPlaceholder
    {
        object GetObject(Document document);
        string GetName();
        Type GetUnderlyingType();
    }
}