using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using BasicCommand.Abstract;

namespace BasicCommand.RibbonItemFactories
{
    public static class PushButtonFactory
    {
        private const string PathPrefix = "pack://application:,,,/";
        private const string PathPostfix = ";component/Resources/";
        private const string ImageFormat = ".png";
        
        /// <summary>
        /// Easy PushButton creation
        /// </summary>
        /// <param name="assembly">Current assembly</param>
        /// <param name="command">ExternalCommand</param>
        /// <returns></returns>
        public static PushButtonData Create(string assembly, Command command)
        {
            try
            {
                var type = command.GetType();
                var assemblyName = type.Assembly.GetName().Name;

                var largeImage = new BitmapImage(
                    new Uri($"{PathPrefix}{assemblyName}{PathPostfix}{command.Picture}{ImageFormat}"));

                return new PushButtonData(command.Name, command.Name, assembly, type.FullName)
                {
                    LargeImage = largeImage,
                    Image = new TransformedBitmap(largeImage, new ScaleTransform(0.5, 0.5)),
                    ToolTip = command.Description,
                    LongDescription = command.Version
                };
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error",
                    $@"During button creation {command.Name} {command.Version} error happened.

{e.Message} | {e.StackTrace}");
                throw;
            }
        }
    }
}