using Autodesk.Revit.DB;
using RevitLookup.Snoop.Collectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitLookup.Snoop
{
    interface IHaveCollector
    {
        void SetDocument(Document document);
    }
}
