using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data.PlaceHolders
{
    interface IObjectToSnoopPlaceholder
    {
        object GetObject(Document document);
        string GetName();
        Type GetUnderlyingType();
    }
}
