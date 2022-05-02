using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace BasicCommand.DefaultExample
{
    public class App : IExternalApplication
    {
        private const string TabName = "CustomTools";
        private const string PanelName = "Graphics";

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(TabName);

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = assembly.Location;

            var graphicsPanel = application.CreateRibbonPanel(TabName, PanelName);

            const string autoViewsCommandName = "Views";
            
            var autoViewsLargeImage = new BitmapImage(
                new Uri($"pack://application:,,,/{assembly.GetName().Name};component/Resources/unknown.png"));

            var autoViewsPushButtonData = new PushButtonData(autoViewsCommandName, autoViewsCommandName, assemblyPath,
                "BasicCommand.DefaultExample.ViewsCreation")
            {
                LargeImage = autoViewsLargeImage,
                Image = new TransformedBitmap(autoViewsLargeImage, new ScaleTransform(0.5, 0.5)),
                ToolTip = "Views creation by elements of selected category",
                LongDescription = "1.0"
            };
            
            
            graphicsPanel.AddItem(autoViewsPushButtonData);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}