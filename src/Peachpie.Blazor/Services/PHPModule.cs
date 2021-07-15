using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
	/// <summary>
	/// Represents the API of phpModule.js providing support functions for Peachpie.Blazor
	/// </summary>
	public class PHPModule : IDisposable
	{
		#region JS function names
		private const string _importCommand = "import";
		private const string _initializeCommand = "initialize";
		private const string _setPHPContextCommand = "setContext";
		private const string _isPostRequestedCommand = "isPostRequested";
		private const string _isFilesPresentedCommand = "isFilesPresented";
		private const string _changeFormsCommand = "turnFormsToClientSide";
		private const string _getfilesCommand = "getFilesData";
		private const string _getPostDataCommand = "getPostData";
		private const string _readAsBase64Command = "readAllFileAsBase64";
		private const string _createUrlObjectCommand = "createUrlObject";
		private const string _downloadFileCommand = "downloadFile";
		private const string _createFileCommand = "createFile";
		#endregion

		private const string _modulePath = "./_content/Peachpie.Blazor/phpModule.js";

		private IJSInProcessObjectReference _moduleRef;

		private DotNetObjectReference<BlazorContext> _ctxRef;

		private PHPModule(IJSInProcessObjectReference module)
		{
			_moduleRef = module;
		}

		private void Initialize()
		{
			_moduleRef.InvokeVoid(_initializeCommand);
		}

		/// <summary>
		/// Imports phpModule.js from the server and initializes it.
		/// </summary>
		public static async Task<PHPModule> CreateAsync(IJSRuntime jsRuntime)
		{
			var module = await (jsRuntime as IJSInProcessRuntime).InvokeAsync<IJSInProcessObjectReference>(_importCommand, _modulePath);
			PHPModule result = new PHPModule(module);

			if (module == null)
				return result;

			result.Initialize();

			return result;
		}

		#region Module exports

		/// <summary>
		/// Sets the context in js runtime, where can be used for calling PHP methods from JS.
		/// </summary>
		public void SetPHPContext(BlazorContext ctx)
		{
			_ctxRef?.Dispose();
			_ctxRef = DotNetObjectReference.Create<BlazorContext>(ctx);

			_moduleRef.InvokeVoid(_setPHPContextCommand, _ctxRef);		
		}

		/// <summary>
		/// Checks if a form with the post method was submited.
		/// </summary>
		public bool IsPostRequest() => _moduleRef.Invoke<bool>(_isPostRequestedCommand);

		/// <summary>
		/// Checks if a form containing files was submited.
		/// </summary>
		public bool isFilesPresented() => _moduleRef.Invoke<bool>(_isFilesPresentedCommand);

		/// <summary>
		/// Turns forms to submit data on a client side. It is handled by Blazor.
		/// </summary>
		public void TurnFormsToClientSide() => _moduleRef.InvokeVoid(_changeFormsCommand);

		/// <summary>
		/// Gets files stored in js runtime. It relates to files, which were loaded or created using Peachpie.Blazor API.
		/// </summary>
		public FormFile[] GetFiles() => _moduleRef.Invoke<FormFile[]>(_getfilesCommand);

		/// <summary>
		/// Gets post data of form.
		/// </summary>
		public Dictionary<string, string> GetPostData() => _moduleRef.Invoke<Dictionary<string, string>>(_getPostDataCommand);

		/// <summary>
		/// Reads a file content.
		/// </summary>
		public async Task<string> ReadFileContentAsBase64(int id) => await _moduleRef.InvokeAsync<string>(_readAsBase64Command, id);

		/// <summary>
		/// Creates an URL object from file by given id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string CreateUrlObject(int id) => _moduleRef.Invoke<string>(_createUrlObjectCommand, id);

		/// <summary>
		/// Donwloads a file by given id.
		/// </summary>
		public void DownloadFile(int id) => _moduleRef.InvokeVoid(_downloadFileCommand, id);

		/// <summary>
		/// Creates a file with given name, data, and content type.
		/// </summary>
		public BrowserFile CreateFile(string data, string name, string contentType) => _moduleRef.Invoke<BrowserFile>(_createFileCommand, data, name, contentType);
		#endregion

		public void Dispose()
		{
			_ctxRef?.Dispose();
			_moduleRef?.Dispose();
		}
	}
}
