# Basic Command

---

## Content

- [Abstract](https://github.com/novikov-ai/revit-basic-command/tree/main/BasicCommand/Abstract)
    - Command (inherit all your plugins from Command Interface instead of IExternalCommand)
    - CommandInfo
  

- [RibbonItemFactories](https://github.com/novikov-ai/revit-basic-command/tree/main/BasicCommand/RibbonItemFactories)
    - PushButtonFactory (use PushButtonFactory.Create(...) for PushButtonData creation)
  

- [ExternalCommands](https://github.com/novikov-ai/revit-basic-command/tree/main/BasicCommand/ExternalCommands)
    - ViewsCreation (example of classic Revit external command)
  

- [App (entry point)](https://github.com/novikov-ai/revit-basic-command/blob/main/BasicCommand/App.cs)

---

## Basic Command way

1. inherit new plugin (external command) from [Command](https://github.com/novikov-ai/revit-basic-command/blob/main/BasicCommand/Abstract/Command.cs)

2. override method RunFunc(...) and implement your logic inside the method 
   
    2.1. if you need modeless window, provide instance of your window to method SetUpModeless(...)
   
    2.2. you don't need to try/catch inside RunFunc(...), but you could throws 3 type of exceptions (see RunFunc(...) summary)

3. override fields: Name, Description, Picture, Version (if you don't override any fields, then their values will set by default)

**Notes**:
- don't forget to increment the version number of your command, when you improve or fix it (field Version)
- it's easy to log every user action (logging errors or collecting statistic):
  - add appropriate logic for your purpose inside Command.Execute(...)
- if you want to create other RibbonItem (eg. PullButton), just create another [RibbonItemFactory](https://github.com/novikov-ai/revit-basic-command/tree/main/BasicCommand/RibbonItemFactories) like a PushButtonFactory and use it in a similar way

*Example: see implementation of [ViewCreation external command](https://github.com/novikov-ai/revit-basic-command/blob/main/BasicCommand/ExternalCommands/ViewsCreation.cs)*

---

## Profit (using Basic Command way)

- 2.6 (!) times fewer lines of code for every external command
  
- easily maintenance of developed commands
  
- minimize your logical errors (you don't need to provide string path to your external command when you call new PushButtonData(...), now you just pass an instance of command to PushButtonFactory.Create(...))

- easy collection logs and statistics (you get opportunity to manage all the external commands from one place - Command.Execute(...) method)

---

## Comparison: Basic Command VS IExternalCommand:

### Basic Command way (abstract class + RibbonItemFactory)

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

### IExternalCommand way (default interface + constructor):
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