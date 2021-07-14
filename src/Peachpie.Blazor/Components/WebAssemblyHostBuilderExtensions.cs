using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Peachpie.Blazor
{
    public static class WebAssemblyHostBuilderExtensions
    {
        /// <summary>
        /// Adds a PHP service providing PHP component navigation.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static void AddPhp(this WebAssemblyHostBuilder builder, Assembly[] assemblies)
        {
            builder.Services.AddSingleton(new PhpComponentRouteManager(assemblies));
            builder.Services.AddSingleton<IPHPService, PHPService>();
        }
    }
}
