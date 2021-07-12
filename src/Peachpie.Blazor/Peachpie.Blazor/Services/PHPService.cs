using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
	public class PHPService : IPHPService
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<PHPService> _logger;
		private PHPModule _phpModule;
		private BlazorContext _ctx;
		private Task _PHPModuleInitialization;

		public PHPService(IJSRuntime jsRuntime, ILoggerFactory loggerFactory)
		{
			_jsRuntime = jsRuntime;
			_loggerFactory = loggerFactory;
			_logger = loggerFactory.CreateLogger<PHPService>();
			_PHPModuleInitialization = PHPModule.CreateAsync(_jsRuntime).ContinueWith( result => _phpModule = result.IsFaulted ? null : result.Result);
		}

		public async Task InitializePHPModuleAsync()
		{
			if (!_PHPModuleInitialization.IsCompleted)
				await _PHPModuleInitialization;
		}

		public PHPModule GetModule()
		{
			if (!_PHPModuleInitialization.IsCompleted)
				throw new Exception("PHP Module is not initialized!");
			else
				return _phpModule;
		}

		public void Dispose()
		{
			_phpModule?.Dispose();
			_ctx?.Dispose();
		}

		public BlazorContext GetActualContext()
		{
			return _ctx;
		}

		public BlazorContext CreateNewContext()
		{
			_ctx?.Dispose();
			_ctx = BlazorContext.Create(_jsRuntime, _loggerFactory, this);

			if (!_PHPModuleInitialization.IsCompleted)
				throw new Exception("PHP Module is not initialized!");
			else
				_phpModule.SetPHPContext(_ctx);

			return _ctx;
		}
	}
}
