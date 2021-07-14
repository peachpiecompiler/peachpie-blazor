using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
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
		private bool _initialized = false;

		private DotNetObjectReference<BlazorContext> _ctxRef;

		private PHPModule(IJSInProcessObjectReference module)
		{
			_moduleRef = module;
		}

		private void Initialize()
		{
			_moduleRef.InvokeVoid(_initializeCommand);
			_initialized = true;
		}

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

		public void SetPHPContext(BlazorContext ctx)
		{
			_ctxRef?.Dispose();
			_ctxRef = DotNetObjectReference.Create<BlazorContext>(ctx);

			_moduleRef.InvokeVoid(_setPHPContextCommand, _ctxRef);		
		}

		public bool IsPostRequest() => _moduleRef.Invoke<bool>(_isPostRequestedCommand);

		public bool isFilesPresented() => _moduleRef.Invoke<bool>(_isFilesPresentedCommand);

		public void TurnFormsToClientSide() => _moduleRef.InvokeVoid(_changeFormsCommand);

		public FormFile[] GetFiles() => _moduleRef.Invoke<FormFile[]>(_getfilesCommand);

		public Dictionary<string, string> GetPostData() => _moduleRef.Invoke<Dictionary<string, string>>(_getPostDataCommand);

		public async Task<string> ReadFileContentAsBase64(int id) => await _moduleRef.InvokeAsync<string>(_readAsBase64Command, id);

		public string CreateUrlObject(int id) => _moduleRef.Invoke<string>(_createUrlObjectCommand, id);

		public void DownloadFile(int id) => _moduleRef.InvokeVoid(_downloadFileCommand, id);

		public BrowserFile CreateFile(string data, string name, string contentType) => _moduleRef.Invoke<BrowserFile>(_createFileCommand, data, name, contentType);
		#endregion

		public void Dispose()
		{
			_ctxRef?.Dispose();
			_moduleRef?.Dispose();
		}
	}
}
