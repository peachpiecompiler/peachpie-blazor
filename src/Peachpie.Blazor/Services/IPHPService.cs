using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
	/// <summary>
	/// The service adds the support for PHP into Blazor.
	/// </summary>
	public interface IPHPService : IDisposable
	{
		public PHPModule GetModule();

		public BlazorContext GetActualContext();

		public BlazorContext CreateNewContext();

		public Task InitializePHPModuleAsync();
	}
}
