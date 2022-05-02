using System.Windows.Markup;

[assembly: XmlnsDefinition("http://revitlookup.com/controls", "RevitLookup.UI.Controls")]
[assembly: XmlnsPrefix("http://revitlookup.com/controls", "rl")]

namespace RevitLookup.UI;

public static class Assembly
{
    /// <summary>
    ///     Empty method to help Revit find a library
    /// </summary>
    /// <remarks>
    ///     By default, Revit does not load the library because it is not referenced from code
    /// </remarks>
    public static void AttachToRevit()
    {
    }
}