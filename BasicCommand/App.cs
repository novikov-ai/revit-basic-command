using System.Reflection;
using Autodesk.Revit.UI;
using BasicCommand.ExternalCommands;
using BasicCommand.RibbonItemFactories;

namespace BasicCommand
{
    public class App : IExternalApplication
    {
        private const string TabName = "CustomTools";
        private const string PanelName = "Graphics";

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(TabName);

            var assemblyPath = Assembly.GetExecutingAssembly().Location;

            var graphicsPanel = application.CreateRibbonPanel(TabName, PanelName);

            var autoViewsPushButtonData = PushButtonFactory.Create(assemblyPath, new ViewsCreation());
            
            graphicsPanel.AddItem(autoViewsPushButtonData);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}