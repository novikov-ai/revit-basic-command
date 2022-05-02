using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using BasicCommand.Abstract;

namespace BasicCommand.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    public class ViewsCreation : Command
    {
        public override string Name => "Views";
        public override string Description => "Views creation by elements of selected category";
        public override string Version => "1.2";

        protected override void RunFunc(ExternalCommandData commandData)
        {
            // SetUpModeless(new ViewsCreationWindow(commandData));
            
            // implement your code for <ViewsCreation> here 
            
            // ...
        }
    }
}