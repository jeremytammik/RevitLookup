using Autodesk.Revit.DB;

namespace RevitLookup.Core;

internal interface IHaveCollector
{
    Document Document { set; }
}