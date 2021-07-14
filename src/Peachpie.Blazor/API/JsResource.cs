using Pchp.Core;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class provides used Javascript function names.
    /// </summary>
    [PhpHidden]
    public static class JsResource
    {
        public static readonly string IsPost = "window.php.isPost";
        public static readonly string IsFiles = "window.php.isFiles";
        public static readonly string getPost = "window.php.forms.getPostData";
        public static readonly string getFiles = "window.php.forms.getFilesData";
        public static readonly string getFileContentAsBase64 = "window.php.files.readAllFileAsBase64";
        public static readonly string createUrlObject = "window.php.files.createUrlObject";
        public static readonly string createFile = "window.php.files.createFile";
        public static readonly string downloadFile = "window.php.files.downloadFile";
        public static readonly string turnFormsToClient = "window.php.forms.turnFormsToClientSide";
    }
}
