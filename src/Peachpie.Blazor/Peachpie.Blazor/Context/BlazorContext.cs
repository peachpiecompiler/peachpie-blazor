using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

[assembly: PhpExtension]

namespace Peachpie.Blazor
{
    /// <summary>
    /// The context is specialized for the Blazor environment.
    /// </summary>
    public class BlazorContext : Context
    {
        private IJSRuntime _js;
        private FileManager _fileManager;
        private ILogger<BlazorContext> _logger;
        private PHPModule _module;

        #region Create
        protected BlazorContext(IServiceProvider services) : base(services)
        {
            Output = BlazorWriter.CreateConsole();
        }

        public static BlazorContext Create(IJSRuntime js, ILoggerFactory loggerFactory, IPHPService phpService)
        {
            var ctx = new BlazorContext(null)
            {
                RootPath = Directory.GetCurrentDirectory(),
                EnableImplicitAutoload = true,
            };

            ctx.WorkingDirectory = ctx.RootPath;
            ctx.InitOutput(null);
            ctx.InitSuperglobals();
            ctx._js = js;
            ctx._module = phpService.GetModule();
            ctx._fileManager = new FileManager(phpService, loggerFactory);
            ctx._logger = loggerFactory.CreateLogger<BlazorContext>();
            ctx.Output = BlazorWriter.CreateConsole();
            
            //
            ctx.AutoloadFiles();

            //
            return ctx;
        }

        public static BlazorContext Create(PhpScriptProvider component) => Create(component.Js, component.LoggerFactory, component.PhpService);
        #endregion

        #region Rendering
        /// <summary>
        /// Sets the context to redirect the script output into the builder.
        /// </summary>
        public void StartRender(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            Output = BlazorWriter.CreateTree(builder);
        }

        /// <summary>
        /// Sets the context to redirect the script output into the console.
        /// </summary>
        public void StopRender()
        {
            Output.Flush();
            Output.Dispose();
            Output = BlazorWriter.CreateConsole();
        }
        #endregion

        #region Set Globals
        /// <summary>
        /// Sets the GET superglobal.
        /// </summary>
        public void SetGet(Dictionary<string, StringValues> query)
        {
            Log.PrintGet(_logger, query);

            foreach (var item in query)
            {
                Get.Add(item.Key, item.Value.ToString());
            }
        }

        /// <summary>
        /// Sets the POST superglobal by using the Javascript interoperability.
        /// </summary>
        public void SetPost()
        {
            if (_module.IsPostRequest())
            {
                var postData = _module.GetPostData();
                Log.PrintPost(_logger, postData);

                foreach (var item in postData)
                {
                    Post.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// Sets the File superglobal by using the Javascript interoperability. It loads all file contents ahead of time in order to use it synchronyously in PHP.
        /// </summary>
        public async Task SetFilesAsync()
        {
            var files = _fileManager.FetchFiles();

            if (files.Count > 0)
            {
                Log.PrintFiles(_logger, files);

                foreach (var item in files)
                {
                    Files.Add(item.fieldName, item);
                }

                await _fileManager.DownloadFilesAsync();
            }
            else
            {
                await Task.CompletedTask;
            }
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// Gets the file content saved in memory.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDownloadFile(int id)
        {
            return _fileManager.GetFileData(id);
        }

        public string CreateUrlObject(int id) => _module.CreateUrlObject(id);

        public BrowserFile CreateFile(string data, string name, string contentType) => _module.CreateFile(data, name, contentType);

        public void DownloadFile(int id) => _module.DownloadFile(id);

        #region JSInterop

        /// <summary>
        /// Calls a PHP function defined in the context by given name.
        /// </summary>
        [JSInvokable]
        public PhpValue CallPHP(string name, string data) => Call(name, data);

        /// <summary>
        /// Calls a Javascript void function.
        /// </summary>
        public void CallJsVoid(string function, params object[] args) => (_js as IJSInProcessRuntime).InvokeVoid(function, args);

        /// <summary>
        /// Calls a Javascript function.
        /// </summary>
        public TResult CallJs<TResult>(string function, params object[] args) => (_js as IJSInProcessRuntime).Invoke<TResult>(function, args);

        /// <summary>
        /// Calls a Javascript void function asynchronyously.
        /// </summary>
        public void CallJsVoidAsync(string function, params object[] args) => _js.InvokeVoidAsync(function, args);

        /// <summary>
        /// Calls a Javascript function asynchronyously.
        /// </summary>
        public ValueTask<TResult> CallJsAsync<TResult>(string function, params object[] args) => _js.InvokeAsync<TResult>(function, args);
        #endregion
    }
}
