# Basic Command

---

## Content

- Abstract
    - Command (inherit all your plugins from Command Interface instead of IExternalCommand)
    - CommandInfo
  

- RibbonItemFactories
    - PushButtonFactory (use PushButtonFactory.Create(...) for PushButtonData creation)
  

- ExternalCommands
    - ViewsCreation (example of classic Revit external command)
  

- App (entry point)

---

## Profit (using Command way)

- 2.6 (!) times fewer lines of code
  
- easily maintenance of developed commands
  
- minimize your logical errors (you don't need to provide string path to your external command when you call new PushButtonData(...), now you just pass an instance of command to PushButtonFactory.Create(...))

- access for collecting logs and statistics (you get opportunity to handle all the external commands from one place - Command.Execute(...) method)

---

## Comparison: Command VS IExternalCommand:

### Command way (abstract class)
#### ViewsCreation : Command
~~~
// lines count: 22

[Transaction(TransactionMode.Manual)]
public class ViewsCreation : Command
{
    public override string Name => "Views";
    public override string Description => "Views creation by elements of selected category";
            
    protected override void RunFunc(ExternalCommandData commandData)
    {
        // SetUpModeless(new ViewsCreationWindow(commandData));
                    
        // implement your code for <ViewsCreation> here 
                    
        // ...
    }
}
~~~

#### App : IExternalApplication
~~~
// lines count: 2

public Result OnStartup(UIControlledApplication application)
{
    ...
    
    var autoViewsPushButtonData = PushButtonFactory.Create(assemblyPath, new ViewsCreation());
    
    graphicsPanel.AddItem(autoViewsPushButtonData);

    ...
}
~~~

### IExternalCommand way (interface):
#### DefaultExample.App : IExternalApplication
~~~
// lines count: 12

public Result OnStartup(UIControlledApplication application)
{
    ...

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
    
    ...
}
~~~

#### DefaultExample.ViewsCreation : IExternalCommand
~~~
// lines count: 51

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
~~~