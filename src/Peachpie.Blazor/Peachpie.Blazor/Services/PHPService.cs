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
		private PHPModule _phpModule;

		public PHPService(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}
		public async Task InitializePHPAsync()
		{
			if (_phpModule == null)
				_phpModule = await PHPModule.CreateAsync(_jsRuntime);
		}

		public PHPModule GetModule() => _phpModule;

		public void Dispose()
		{
			_phpModule?.Dispose();
		}
	}
}
