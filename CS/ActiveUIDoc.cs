using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB; 

namespace RevitLookup
{
  public class ActiveDoc
  {
    private static Document m_doc;
    public static Autodesk.Revit.DB.Document Doc
    {
      get { return m_doc; }
      set { m_doc = value; }
    }
  }
}
