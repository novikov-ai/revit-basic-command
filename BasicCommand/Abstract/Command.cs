using System;
using System.ComponentModel;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BasicCommand.Abstract
{
    public abstract class Command : IExternalCommand, CommandInfo
    {
        /// <summary>
        /// Displayed name of RibbonItem for user.
        /// </summary>
        public virtual string Name => "Unnamed";

        /// <summary>
        /// Displayed tooltip of RibbonItem for user.
        /// </summary>
        public virtual string Description => "No description";

        /// <summary>
        /// Image of the RibbonItem at custom RibbonPanel.
        /// </summary>
        public virtual string Picture => "unknown";

        /// <summary>
        /// Version of the RibbonItem. Displayed like long description.
        /// </summary>
        public virtual string Version => "1.0";

        private static Window _window = null;
        private bool _isWindowModal = true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                RunFunc(commandData);

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                if (!_isWindowModal)
                {
                    _isWindowModal = true;
                    _window = null;
                }

                switch (e)
                {
                    case OperationCanceledException:
                    case Autodesk.Revit.Exceptions.OperationCanceledException:
                    {
                        return Result.Cancelled;
                    }
                    case WarningException:
                    {
                        TaskDialog.Show("Warning", e.Message);
                        break;
                    }

                    default:
                    {
                        TaskDialog.Show("Error", e.Message);
                        break;
                    }
                }

                return Result.Failed;
            }
        }

        /// <summary>
        /// [INFO]:
        /// 1. Implement your plugin logic inside the method.
        /// 2. You don't need to put your logic inside try-catch.
        /// 
        /// [EXCEPTIONS]:
        /// 1. Use throw new  OperationCanceledException if you need handler for Canceled situation.
        /// 2. Use throw new  WarningException if you need handler for Warning situation.
        /// 3. Use throw new  Exception if you need handler for anything else.
        /// 
        /// [OPTIONAL]:
        /// 1. Invoke SetUpModeless(Window window) if you need modaless window.
        /// 2. Invoke TurnOffStatistics() if you're developing a new plugin and debugging it. After release please remove this method.
        /// </summary>
        protected virtual void RunFunc(ExternalCommandData commandData)
        {
        }

        /// <summary>
        /// Use this method to set up modeless window.
        /// </summary>
        /// <param name="window"></param>
        protected void SetUpModeless(Window window)
        {
            _isWindowModal = false;

            if (_window is null)
            {
                _window = window;
                _window.Show();

                _window.Closed += (o, e) => { _window = null; };
            }
            else
            {
                _window.Activate();
                _window.WindowState = WindowState.Normal;
            }
        }
    }
}