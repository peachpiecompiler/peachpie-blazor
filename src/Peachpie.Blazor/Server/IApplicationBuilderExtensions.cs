using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Provides navigation to static resources defined in appsettings.json. It finds the AdditionalStaticWebAssets section, where are defined Path of the assets and BasePath used in a browser.
        /// </summary>
        public static void UseAdditionalWebStaticAssets(this IApplicationBuilder app, IConfiguration config)
        {
            foreach (var item in config.GetSection("AdditionalStaticWebAssets").GetChildren())
            {
                string path = item["Path"];
                if (!Path.IsPathRooted(item["Path"]))
                    path = Path.Combine(Directory.GetCurrentDirectory(), item["Path"]);

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = item["BasePath"]
                });
            }
        }
    }
}
