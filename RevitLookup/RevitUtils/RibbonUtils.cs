using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace RevitLookup.RevitUtils
{
    public static class RibbonUtils
    {
        /// <summary>
        ///     Adds a button to the ribbon
        /// </summary>
        public static PulldownButton AddPullDownButton(this RibbonPanel panel, string name, string buttonText)
        {
            var pushButtonData = new PulldownButtonData(name, buttonText);
            return (PulldownButton) panel.AddItem(pushButtonData);
        }

        public static PushButton AddPushButton(this PulldownButton pullDownButton, Type command, string commandName, string buttonText)
        {
            var pushButtonData = new PushButtonData(commandName, buttonText, Assembly.GetAssembly(command).Location, command.FullName);
            return pullDownButton.AddPushButton(pushButtonData);
        }

        /// <summary>
        ///     Creates a panel in the Add-ins tab.
        /// </summary>
        public static RibbonPanel CreateRibbonPanel(UIControlledApplication application, string panelName)
        {
            var ribbonPanels = application.GetRibbonPanels(Tab.AddIns);
            return CreateRibbonPanel(application, panelName, ribbonPanels);
        }

        /// <summary>
        ///     Adds a 16x16-96 dpi image from the URI source
        /// </summary>
        public static void SetImage(this RibbonButton button, string uri)
        {
            button.Image = new BitmapImage(new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        ///     Adds a 32x32-96 dpi image from the URI source
        /// </summary>
        public static void SetLargeImage(this RibbonButton button, string uri)
        {
            button.LargeImage = new BitmapImage(new Uri(uri, UriKind.Relative));
        }

        private static RibbonPanel CreateRibbonPanel(UIControlledApplication application, string panelName, IEnumerable<RibbonPanel> ribbonPanels)
        {
            var ribbonPanel = ribbonPanels.FirstOrDefault(panel => panel.Name.Equals(panelName));
            return ribbonPanel ?? application.CreateRibbonPanel(panelName);
        }
    }
}