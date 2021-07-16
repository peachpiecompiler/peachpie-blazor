using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    /// <summary>
    /// Defines a context preservation type.
    /// </summary>
    public enum SessionLifetime { Persistant, OnNavigationChanged }

    /// <summary>
    /// Defines the component type.
    /// </summary>
    public enum PhpScriptProviderType { Router, ScriptProvider, Script }

    /// <summary>
    /// The class provides PHP script and PHP components navigation and evaluation.
    /// </summary>
    public class PhpScriptProvider : IComponent, IHandleAfterRender, IDisposable
    {
        #region Parameters
        [Parameter]
        public PhpScriptProviderType Type { get; set; } = PhpScriptProviderType.Router;

        [Parameter]
        public string ScriptName { get; set; } = "index.php";

        [Parameter]
        public RenderFragment NotFound { get; set; } = delegate (Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { };

        [Parameter]
        public RenderFragment Navigating { get; set; } = delegate (Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { };

        [Parameter]
        public SessionLifetime ContextLifetime { get; set; } = SessionLifetime.OnNavigationChanged;
        #endregion

        #region Injection
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        private PhpComponentRouteManager RouteManager { get; set; }

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private INavigationInterception NavigationInterception { get; set; }

        [Inject]
        public IPHPService PhpService { get; set; }
        #endregion

        private RenderHandle _renderHandle;
        private ILogger<PhpScriptProvider> _logger;
        private bool _navigationInterceptionEnabled;
        private BlazorContext _ctx;
        private bool _disposed = false;
        private bool _firstRendering = true;
        PHPModule _module;

        #region IComponent
        void IComponent.Attach(RenderHandle renderHandle)
        {
            _logger = LoggerFactory.CreateLogger<PhpScriptProvider>();
            Log.Attach(_logger);

            _renderHandle = renderHandle;
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        async Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            
            await PhpService.InitializePHPModuleAsync();
            _module = PhpService.GetModule();

            Log.SetParameters(_logger, _firstRendering, Type, ContextLifetime);

            if (_firstRendering)
            {
                _firstRendering = false;
                _ctx = PhpService.CreateNewContext();
                Refresh();
            }
        }
        #endregion

        private void Refresh()
        {
            // Determines the script name based on URL.
            if (Type != PhpScriptProviderType.Script)
            {
                var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                if (relativeUri.IndexOf('?') > -1)
                    relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));

                ScriptName = String.IsNullOrWhiteSpace(relativeUri) ? "index.php" : relativeUri;
            }

            // Sets GET superglobal
            if (Uri.TryCreate(NavigationManager.Uri, UriKind.Absolute, out Uri uri))
            {
                var values = QueryHelpers.ParseQuery(uri.Query);
                if (values.Count > 0)
                    _ctx.SetGet(values);
            }
                
            _ctx.SetPost();
            var task = _ctx.SetFilesAsync();

            // Waits until the files are loaded into memory.
            if (task.Status != TaskStatus.RanToCompletion)
            {
                Log.BusyNavigation(_logger);
                if (Navigating != null)
                    _renderHandle.Render(Navigating);

                task.ContinueWith((Task t) => Render(ScriptName));
            }
            else
            {
                Render(ScriptName);
            }
        }

        private void Render(string ScriptName)
        {
            Context.ScriptInfo script = default;
            
            // If .php exists, it is a script, otherwise it is a component
            if (ScriptName.EndsWith(".php"))
                script = Context.TryGetDeclaredScript(ScriptName);

            if (!script.IsValid)
            {
                var segments = ScriptName.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var result = RouteManager.Match(segments);

                if (result.IsMatch)
                {
                    Log.ComponentNavigation(_logger, ScriptName);
                    _renderHandle.Render((builder) => {
                        builder.OpenComponent(0, result.MatchedRoute.Handler);
                        builder.CloseElement();
                    });
                }
                else
                {
                    Log.NavigationFailed(_logger, ScriptName);
                    _renderHandle.Render(NotFound);
                }
            }
            else
            {
                Log.ScriptNavigation(_logger, ScriptName);
                _renderHandle.Render((builder) => script.Evaluate(_ctx, builder));
            }
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            Log.Navigation(_logger, args.Location, _disposed);

            if (_disposed)
                return;

            if (ContextLifetime == SessionLifetime.OnNavigationChanged)
            {
                _ctx = PhpService.CreateNewContext();
            }

            Refresh();
        }

        #region IHandleAfterRender
        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled && Type == PhpScriptProviderType.Router)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            //_ctx.CallJsVoid(JsResource.turnFormsToClient);
            _module.TurnFormsToClientSide();

            return Task.CompletedTask;
        }
        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
            Log.Dispose(_logger);
            _disposed = true;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
        #endregion
    }
}
