using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Core;

/// <summary>
///     The class contains wrapping methods for working with the Revit API.
/// </summary>
public static class RevitApi
{
    public static UIApplication UiApplication { get; set; }
    public static UIDocument UiDocument => UiApplication.ActiveUIDocument;
    public static Document Document => UiApplication.ActiveUIDocument.Document;
}