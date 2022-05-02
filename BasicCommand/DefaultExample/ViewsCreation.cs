using System;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BasicCommand.DefaultExample
{
    [Transaction(TransactionMode.Manual)]
    public class ViewsCreation : IExternalCommand
    {
        // if you need modeless window
        private static Window _window = null;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // implement your code for <ViewsCreation> here 

                // ...

                // if you need modeless window
                if (_window is null)
                {
                    // _window = new ViewsCreationWindow(commandData)
                    _window = new Window();
                    _window.Show();

                    _window.Closed += (o, e) => { _window = null; };
                }
                else
                {
                    _window.Activate();
                    _window.WindowState = WindowState.Normal;
                }

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", e.Message);
                return Result.Failed;
            }
        }
    }
}