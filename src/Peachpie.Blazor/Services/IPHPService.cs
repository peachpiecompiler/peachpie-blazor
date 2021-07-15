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
		/// <summary>
		/// Gets a js module providing supporting functions for Peachpie.Blazor.
		/// </summary>
		public PHPModule GetModule();

		/// <summary>
		/// Gets a context set by <see cref="PhpScriptProvider"/> or <see cref="PhpComponent"/>.
		/// </summary>
		/// <returns>Null, if the context has not been set else <see cref="BlazorContext"/></returns>
		public BlazorContext GetActualContext();

		/// <summary>
		/// Creates a new context, which is set as the actual context of the session.
		/// </summary>
		/// <returns></returns>
		public BlazorContext CreateNewContext();

		/// <summary>
		/// Initializes PHPModule (imports phpModule.js from the server).
		/// </summary>
		public Task InitializePHPModuleAsync();
	}
}
