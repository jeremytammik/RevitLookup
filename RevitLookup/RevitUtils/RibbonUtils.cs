using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace RevitLookup.RevitUtils
{
    public static class RibbonUtils
    {
        public static PushButton AddPushButton(this PulldownButton pullDownButton, Type command, string commandName, string buttonText)
        {
            var pushButtonData = new PushButtonData(commandName, buttonText, Assembly.GetAssembly(command).Location, command.FullName);
            return pullDownButton.AddPushButton(pushButtonData);
        }

        public static RibbonPanel CreatePanel(this UIControlledApplication application, string panelName)
        {
            var ribbonPanels = application.GetRibbonPanels(Tab.AddIns);
            return CreatePanel(application, panelName, ribbonPanels);
        }

        private static RibbonPanel CreatePanel(UIControlledApplication application, string panelName, IEnumerable<RibbonPanel> ribbonPanels)
        {
            var ribbonPanel = ribbonPanels.FirstOrDefault(panel => panel.Name.Equals(panelName));
            return ribbonPanel ?? application.CreateRibbonPanel(panelName);
        }
    }
}