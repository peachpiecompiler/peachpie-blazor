using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class represents a base class for inheriting in PHP.
    /// </summary>
    public abstract class PhpComponent : ComponentBase, IDisposable
    {
        protected Context _ctx;

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        public IPHPService PhpService { get; set; }

		public void Dispose()
		{}

		protected sealed override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            BuildRenderTree(new PhpTreeBuilder(builder, this));
        }

        protected abstract void BuildRenderTree(PhpTreeBuilder builder);

		protected override void OnInitialized()
		{
			base.OnInitializedAsync();
            PhpService.InitializePHPModuleAsync().Wait();
            _ctx = PhpService.GetActualContext() ?? PhpService.CreateNewContext();
        }
    }
}
