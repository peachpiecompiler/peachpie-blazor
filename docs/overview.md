# Overview

Integration of Blazor and PeachPie enables executing PHP scripts on a client side without additional requests to a server. The integration comprises `Peachpie.Blazor` and `Peachpie.Blazor.Sdk` providing helper structures to make PHP a valuable part of client side web development. Everything is built on top of the Blazor platform, so you don't need any browser extensions.

## Requirements

.NET 5 SDK ([Download .NET 5.0 (Linux, macOS, and Windows) (microsoft.com)](https://dotnet.microsoft.com/download/dotnet/5.0))

## Quick Start

1. Install **Peachpie.Templates** by `dotnet new -i "Peachpie.Templates::*"`
2. Choose the template based on your intention:
   - **blazor-hybrid**- A simple PHP website running in browser
   - **blazor-web** - A simple Blazor website combining PHP and Razor Pages
3. Create the project by `dotnet new project-name`
4. Add Razor pages to **BlazorApp.Client** or PHP scripts to **PHPScripts** (Optional)
5. Launch the **Blazor.Server** by `dotnet run --project BlazorApp\Server`
6. Access [https://localhost:5001](https://localhost:5001/)

## Demo

> Demo examples can be found in [src/Tests/Examples](https://github.com/peachpiecompiler/peachpie-blazor/tree/master/src/Tests/Examples) folder of the official repository [peachpiecompiler/peachpie-blazor](https://github.com/peachpiecompiler/peachpie-blazor).

You can see a simple PHP web application, which is created as a Peachpie project and downloaded to a client by only one initial request below. The next navigation in the website is offline, using the Blazor environment.

<img src=".\images\video1.gif" alt="Demo" style="zoom: 67%;" />

## Remarks

- The integration provides a lot of advanced features like JS PHP interoperability or transparent evaluating of web forms without request to a server.
- There are many user scenarios, which are interesting and can be found in the examples.
- See also other documentation files describing all features of the library.

## Related links

- Repository [peachpiecompiler/peachpie-blazor](https://github.com/peachpiecompiler/peachpie-blazor/blob/master/README.md)

- https://www.nuget.org/packages/Peachpie.Blazor.Sdk/

  <a href="https://www.nuget.org/packages/Peachpie.Blazor.Sdk/"><img src="https://img.shields.io/nuget/v/Peachpie.Blazor.Sdk.svg?style=flat"></a>