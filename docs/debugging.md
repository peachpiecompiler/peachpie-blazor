# Debugging PHP in Blazor

From now on, you can debug your PHP code running in the browser in Visual Studio ! You just run the *Blazor.Server* in `DEBUG` mode and set the breakpoint to any line of your PHP code. The **Peachpie.Blazor.Sdk** then sets the environment to enable PHP debugging.       

![debug](E:\OwnCode\Github\peachpie-blazor\docs\images\debug.png)

## Logging 

Because `PhpComponent` and `PhpScriptProvider` have many stages, we provide helpful logs describing the behaviour of the components. Logs are displayed in the browser console by default. If you want to prohibit logging, just add `builder.Logging.SetMinimumLevel(LogLevel.None);` to *Program.cs* in **Blazor.Client** project.

![logs](E:\OwnCode\Github\peachpie-blazor\docs\images\log.png)