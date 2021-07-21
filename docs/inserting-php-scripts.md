# Inserting PHP scripts

Before we will start to talk about using classes provided by `Peachpie.Blazor`, we introduce the project background and steps to create it. The reader can skip it and use prepared templates, which can be arbitrarily customized. The instructions can be found in the [overview](https://docs.peachpie.io/scenarios/blazor/overview/).

## Starting with empty solution

Create a standard Blazor WebAssembly application (with .NET 5) and tick ASP.NET Core hosted.

<img src=".\images\creating-web-assembly.png" alt="VS dialog" style="zoom:50%;" />

We have to add a package reference for `Peachpie.Blazor` library to `BlazorApp.Client` and `BlazorApp.Server` projects in order to be able to use helper classes providing script navigation and execution.

<img src=".\images\nuget.png" alt="VS dialog" style="zoom:50%;" />

Now, we have to add support for PHP in the `WebAssemblyHost` of `BlazorApp.Client` project. Because of Blazor specification, we have to add reference on a type defined in the additional assembly in order to make the assembly be downloaded with the `BlazorApp.Client`. 

> *`BlazorApp/Client/Program.cs`:*

```c#
...
using Peachpie.Blazor;
...
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");

    // Add PHP
    builder.AddPhp(new[] { typeof(typeInPHPScripts).Assembly});

    await builder.Build().RunAsync();
}
...
```

The last change in the projects is to redirect navigation of `https://sth/sth.php`  to `index.html` on the server side, which is done by modifying the APS.NET pipeline in the following way:

> *`BlazorApp/Server/Startup.cs`:*

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	...
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("index.html");
        endpoints.MapFallbackToFile("/{**.php}", "index.html");
    });
}
```

Add a new PeachPie project, where we will have our PHP scripts.

> *`PHPScripts.msbuildproj`:*

```xml
<Project Sdk="Peachpie.Blazor.Sdk/1.0.11936">
 <PropertyGroup>
    <OutputType>library</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <ProduceReferenceAssembly>false</ProduceReferenceAssembly>
  </PropertyGroup>
</Project>
```

Now, we are ready to add PHP scripts to the web application. The server can be launched by `dotnet run --project BlazorApp\Server`.

## Inserting PHP scripts

We have two options for how to insert PHP content into a Blazor web application. The first way uses `PhpScriptProvider` class, which is a Blazor component. This component can be configured to execute and render specified script or can automatically find the best-matched script path with the current URL. The second choice is to inherit `PhpComponent` class in PHP, which enables to access Blazor API.

> Warning: When you have more than one instance of `PhpScriptProvider`  or `PhpComponent` in the running app (except in specific situations), the JS PHP interoperability will don't works properly (But the interop is an advanced feature, so you can don't bother with it now). This is a limitation of the current implementation. 

### Inserting PHP scripts into Razor pages

Thanks to `IComponent` interface, we can insert `PhpScriptProvider` into Razor page as a normal Blazor component. We can configure a set of PHP scripts, which we want to be executed. The most simple configuration is to navigate just one script by the class. 

The provider finds PHP scripts by their relative paths to the project folder. So, when we have `https://[server]/folder1/folder2/file.php` URL, the provider tries to find the script with a relative path `folder1/folder2/file.php` to the Peachpie project.   

##### Navigating only one PHP script 

> *`Index.razor`:*

```razor
@page "/index.php"
@using Peachpie.Blazor

<!--Other Blazor content (optional)-->

<PhpScriptProvider Type="@PhpScriptProviderType.Script" ScriptName="index.php"> 
	<Navigating>          
		<p>Navigating</p>     
	</Navigating>         
	<NotFound>            
		<p>Script not found</p
	</NotFound>           
</PhpScriptProvider>

<!--Other Blazor content (optional)-->
```

Now, we add a new file named `index.php` to `PHPScripts` project and launch the server. We can access https://index.html, which is handled by Blazor, and `Peachpie.Blazor` library executing the script and rendering its output as page content.

##### Navigating a set of PHP scripts

When we want to navigate more PHP scripts, we just change the `Type` parameter of `PhpScriptProvider` to `PhpScriptProviderType.Provider`. It enables to navigate PHP scripts based on matching the current URL with script paths.

So, when we want to navigate all scripts contained in the `folder1` folder of `PHPScripts` project, we just add the following Razor page to the `BlazorApp.Client` project:

```razor
@page "/folder1/{*sth}"
@using Peachpie.Blazor

<PhpScriptProvider Type="@PhpScriptProviderType.Provider"> 
	<Navigating>          
		<p>Navigating</p>     
	</Navigating>         
	<NotFound>            
		<p>Script not found</p
	</NotFound>           
</PhpScriptProvider>

@code
{
    [Parameter] public string sth { get; set; }
}
```

### PHP Script router

Navigating only PHP scripts is the last option that the library offers. We set the provider as a root component and change the `Type` parameter to `PhpScriptProviderType.Router` handling all navigation events. We can see an example below.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    // Add PHP
    builder.AddPhp(new[] { typeof(force).Assembly });
    
    // Set the provider as a root component
    builder.RootComponents.Add(typeof(PhpScriptProvider), "#app");

    await builder.Build().RunAsync();
}
```

## Make your Blazor component in PHP 

Because PeachPie enables inheriting C# classes in PHP. We implemented a `PhpComponent` class representing the classic Blazor component adjusted to support PHP. It provides us full support of Blazor API though `ComponentBase` interface. Because the Blazor API is not fully compatible with PHP language, the helper classes were implemented to help with that.

```php
<?php

#[\Microsoft\AspNetCore\Components\RouteAttribute("/MyComponent")]
class MyComponent extends \Peachpie\Blazor\PhpComponent
{	
	private $infoTag;

	public  function BuildRenderTree($builder) : void 
	{
		$this->infoTag->writeWithTreeBuilder($builder, 0);
	}

	public function OnInitialized() : void 
	{
		parent::OnInitialized();

		$this->infoTag = new \Peachpie\Blazor\Tag("p");
        $this->infoTag->content[] = new \Peachpie\Blazor\Text("Hello world");
	}
}
```

For those, whose are familiar with Blazor, the code above should be understandable. It is a common Blazor component implemented in the PHP language. There is a collection of helper classes like `Tag` helping to render the page content. You can find a full reference API on [https://docs.peachpie.io/scenarios/blazor/api-reference/](https://docs.peachpie.io/scenarios/blazor/api-reference/) , which will tell you more about these helpers. 

At first glance, this way of adding PHP to Blazor can be considered difficult to use, but it enables utilizing a smart diffing algorithm during rendering. Hence, we can also create render-demanding applications like games by using this way. There is an [example](https://github.com/peachpiecompiler/peachpie-blazor/tree/dev/docs/src/Tests/Examples/WebGame) of a simple Asteroids-like game, which is fully implemented by PHP and executed as a client-side application. We can see a screenshot from this game below. 

<img src=".\images\asteroids.png" alt="screenshot" style="zoom:60%;" />

