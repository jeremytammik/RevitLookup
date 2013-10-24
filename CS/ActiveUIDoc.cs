using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup
{
  public class ActiveDoc
  {
    private static UIApplication m_uiApp;
    public static Autodesk.Revit.UI.UIApplication UIApp
    {     
      set { m_uiApp = value; }
    }
    public static Autodesk.Revit.DB.Document Doc
    {
      get { return m_uiApp.ActiveUIDocument.Document; }
    }
  }
}
