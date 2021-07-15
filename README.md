<h1>
Blazor plus Peachpie
</h1>
**PHP as a client side language**

> The library and SDK aren't presented on nuget.org yet. But if you want to already try it, just compile the **Peachpie.Blazor.Sdk** and **Peachpie.Blazor** and add the nuget paths to NuGet.Config file.

## What is it ?

This repository contains **Peachpie.Blazor** library and **Peachpie.Blazor.Sdk** to enable making websites with PHP together with Blazor on a client side. With these components, you can write just PHP script, which are transparently navigated and managed.

You can see usage demonstration bellow. 

<img src=".\docs\images\video1.gif" alt="Demo" style="zoom: 67%;" />

## Why it is useful ?

There are potential scenarios of usage:

- You want to move a PHP website to a client side for saving server resources.
- You have a Blazor website and want to write a part of the website in the PHP language.
- You want to use PHP libraries on a client side.
- PHP and Blazor team can work together to make awesome website using .NET and PHP.   

## How to get started ?

You can start right now.

Steps:

1. Install .NET 5.0 SDK
2. Install **Peachpie.Blazor.Templates** by ```dotnet new -i "Peachpie.Blazor.Templates::*"```
3. Choose a template depending on your intention: 

	- **Peachpie Blazor Web** - A simple PHP website running in browser 

	- **Peachpie Blazor Hybrid** - A simple Blazor website combining PHP and Razor Pages


4. Create the project by ```dotnet new project-name```

5. Add Razor pages to **BlazorApp.Client** or PHP scripts to **PHPScripts** (Optional)

6. Launch the **Blazor.Server** by navigating to its folder and run ```dotnet run```

7. Access https://localhost:5001

<img src=".\docs\images\Structure.png" alt="Solution structure" style="zoom: 67%;" />

## Features

- Transparent navigation of PHP scripts by URL

- Transparent rendering of HTML pages generated from PHP

- Full support of $\_GET, $\_POST, and $\_FILES

- Simulation of a server on a client side

- Forms are handled by PHP on a client side

- Possibility of persistent PHP context to save client session on a client side
- Inserting PHP scripts to Razor pages
- PHP, C#, JavaScript interoperability

## Future plans

There is a rough overview of the possible plans for the future:

- Make rendering with the **PhpScriptProvider** enough efficient in order to remove **PhpComponent** and make the usage even more transparent.
- Figure out the support for databases.
- Move WordPress to a client side.

## Documentation

Detailed information about the solution, the library, and the functionality can be found in *docs* folders. Advanced features of the integration are shown there. There can be some missing information about any part of the whole project. If you can ask about something, feel free to contact me and I will add these information to the folder for future interested people. 

## Release

This is a first version of the integration. The API and functionality might still change to improve the advantages. The sources compilation should work, feel free to contact me (husaktomas98@google.com) to get more information about this project.
