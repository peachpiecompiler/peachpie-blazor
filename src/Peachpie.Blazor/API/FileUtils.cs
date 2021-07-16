using Pchp.Core;
using System;

namespace Peachpie.Blazor
{

    /// <summary>
    /// The class represents information about saved file in a browser memory.
    /// </summary>
    [PhpType]
    public class BrowserFile
    {
        public int id { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
    }

    /// <summary>
    /// The class represents information about file obtained from an HTML form.
    /// </summary>
    [PhpType]
    public class FormFile : BrowserFile
    {
        public string fieldName {get; set;}
    }

    /// <summary>
    /// The class enables to work with files from PHP.
    /// </summary>
    public static class FileUtils
    {

        /// <summary>
        /// Gets already fetched file content from the context.
        /// </summary>
        public static PhpString GetBrowserFileContent(Context ctx, int id)
        {
            return new PhpString ((ctx as BlazorContext).GetDownloadFile(id));
        }

        /// <summary>
        /// Creates an URL object from a file specified by id and returns the URL.
        /// </summary>
        public static string CreateUrlObject(Context ctx, int id)
        {
            return (ctx as BlazorContext).CreateUrlObject(id); 
        }

        /// <summary>
        /// Creates a file with the given content and type and returns information about it.
        /// </summary>
        public static BrowserFile CreateFile(Context ctx, string data, string name, string contentType)
        {
            return (ctx as BlazorContext).CreateFile(data, name, contentType);
        }

        /// <summary>
        /// Download a file specified by id to a client.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="id"></param>
        public static void DownloadFile(Context ctx, int id)
        {
            (ctx as BlazorContext).DownloadFile(id);
        }
    }
}
