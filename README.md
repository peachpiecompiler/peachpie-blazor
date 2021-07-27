<h1>
PHP Tools for Blazor
</h1>


**PHP as a client side language**

<p align="center">
<a href="https://www.nuget.org/packages/Peachpie.Blazor.Sdk/"><img src="https://img.shields.io/nuget/v/Peachpie.Blazor.Sdk.svg?style=flat"></a>
<a href="https://docs.peachpie.io/scenarios/blazor/overview/"><img src="https://img.shields.io/badge/docs-peachpie.blazor.io-green.svg"></a> 
<a href="https://docs.peachpie.io/scenarios/blazor/overview/"><img src="https://img.shields.io/badge/license-MIT-blue"></a>
</p>


## What is it?

This repository contains **Peachpie.Blazor** library and **Peachpie.Blazor.Sdk** to enable making websites with PHP together with Blazor on a client side. With these components, you can write just PHP scripts, which are transparently navigated and managed.

You can see the usage demonstration below. 

<img src=".\docs\images\video1.gif" alt="Demo" style="zoom: 67%;" />

## Why is it useful?

There are potential scenarios of usage:

- You want to move a PHP website to a client side to save server resources.
- You have a Blazor website and want to write a part of the website in the PHP language.
- You want to use PHP libraries on the client side.
- PHP and Blazor team can work together to make an awesome website using .NET and PHP.  

## How to get started?

You can start right now.

Steps:

1. Install **Peachpie.Templates** by `dotnet new -i "Peachpie.Templates::*"`
2. Choose the template based on your intention:
   - **blazor-hybrid**- A simple PHP website running in browser
   - **blazor-web** - A simple Blazor website combining PHP and Razor Pages
3. Create the project by `dotnet new project-name`
4. Add Razor pages to **BlazorApp.Client** or PHP scripts to **PHPScripts** (Optional)
5. Launch the **Blazor.Server** by `dotnet run --project BlazorApp\Server`
6. Access [https://localhost:5001](https://localhost:5001/)

<img src=".\docs\images\Structure.png" alt="Solution structure" style="zoom: 60%;" />

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

- Make rendering with the **PhpScriptProvider** efficient enough in order to remove **PhpComponent** and make the usage even more transparent.
- Figure out the support for databases.
- Move WordPress to a client side.

## Documentation

Detailed information about the solution, the library, and the functionality can be found in *docs* folders. Advanced features of the integration are shown there. There can be some missing information about any part of the whole project. If you can ask about something, feel free to contact me, and I will add this information to the folder for future interested people. 

## Release

This is the first version of the integration. The API and functionality might still change to improve the advantages. The sources compilation should work, feel free to contact me ([husaktomas98@gmail.com](mailto:husaktomas98@google.com)) to get more information about this project.
