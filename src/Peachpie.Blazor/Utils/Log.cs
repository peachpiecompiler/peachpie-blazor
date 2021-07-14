using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Peachpie.Blazor
{
	/// <summary>
	/// The class provides logging of component states.
	/// </summary>
	public static class Log
	{
        #region PhpScriptProvider
        private static readonly Action<ILogger, Exception> _attach = LoggerMessage.Define(LogLevel.Information, new EventId(1, "Attaching"), "Attach method was invoked");
		private static readonly Action<ILogger,bool,PhpScriptProviderType,SessionLifetime, Exception> _setParameters = LoggerMessage.Define<bool,PhpScriptProviderType,SessionLifetime>(LogLevel.Information, new EventId(2, "SettingParameters"), "SetParameters method was invoked.\nFirst renering={Rendered}\nProvider type={Type}\nContext mode={Mode}");
		private static readonly Action<ILogger, Exception> _busyNavigating = LoggerMessage.Define(LogLevel.Information, new EventId(3, "BusyNavigating"), "Navigating is busy.");
		private static readonly Action<ILogger,string, Exception> _navigatingComponent = LoggerMessage.Define<string>(LogLevel.Information, new EventId(4, "NavigatingCompoent"), "Component {Name} is being rendered");
		private static readonly Action<ILogger,string, Exception> _navigatingFailed = LoggerMessage.Define<string>(LogLevel.Information, new EventId(5, "NavigatingFailed"), "Navigating {Path} failed");
		private static readonly Action<ILogger, string, Exception> _navigatingScript = LoggerMessage.Define<string>(LogLevel.Information, new EventId(6, "NavigatingScript"), "Script {Name} is being rendered");
		private static readonly Action<ILogger, string, bool, Exception> _navigating = LoggerMessage.Define<string, bool>(LogLevel.Information, new EventId(7, "Navigating"), "Navigating to {Path} started. PhpScript provider disposed = {Disposed}");
		private static readonly Action<ILogger, Exception> _dispose = LoggerMessage.Define(LogLevel.Information, new EventId(8, "Disposing"), "PhpSriptProvider is disposed");

		public static void Attach(ILogger logger)
		{
			_attach(logger, null);
		}

		public static void SetParameters(ILogger logger, bool firstRendering, PhpScriptProviderType type, SessionLifetime lifetime)
		{
			_setParameters(logger, firstRendering, type, lifetime, null);
		}

		public static void BusyNavigation(ILogger logger)
		{
			_busyNavigating(logger, null);
		}

		public static void ComponentNavigation(ILogger logger, string name)
		{
			_navigatingComponent(logger, name, null);
		}

		public static void NavigationFailed(ILogger logger, string name)
		{
			_navigatingFailed(logger, name, null);
		}

		public static void ScriptNavigation(ILogger logger, string name)
		{
			_navigatingScript(logger, name, null);
		}

		public static void Navigation(ILogger logger, string address, bool disposed)
		{
			_navigating(logger, address, disposed, null);
		}

		public static void Dispose(ILogger logger)
		{
			_dispose(logger, null);
		}
		#endregion

		#region BlazorContext
		private static readonly Action<ILogger, string, string, Exception> _dictionary = LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(9, "PrintingDictionary"), "Added {Method} data\n{Values}");

		public static void PrintFiles(ILogger logger, List<FormFile> files)
		{
			var payload = "";
			foreach (var item in files)
			{
				payload += $"fieldName: {item.fieldName}, id : {item.id}, name : {item.name}, size : {item.size}, type : {item.type}\n";
			}
			_dictionary(logger, "FILES", payload, null);
		}

		public static void PrintGet(ILogger logger, Dictionary<string, StringValues> values)
		{
			var payload = "";
			foreach (var item in values)
			{
				payload += $"key : {item.Key}, value : {item.Value.ToString()}\n";
			}
			_dictionary(logger, "GET", payload, null);
		}

		public static void PrintPost(ILogger logger, Dictionary<string, string> values)
		{
			var payload = "";
			foreach (var item in values)
			{
				payload += $"key : {item.Key}, value : {item.Value.ToString()}\n";
			}
			_dictionary(logger, "POST", payload, null);
		}
		#endregion

		#region FileManager
		private static readonly Action<ILogger, string, Exception> _download = LoggerMessage.Define<string>(LogLevel.Information, new EventId(10, "DownloadedFiles"), "File {name} downloaded.");

		public static void DownloadFile(ILogger logger, FormFile file)
		{
			_download(logger, file.name,null);
		}
		#endregion


	}
}
