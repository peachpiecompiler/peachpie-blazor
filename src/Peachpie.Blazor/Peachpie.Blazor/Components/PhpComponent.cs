using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Pchp.Core;
using System;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class represents a base class for inheriting in PHP.
    /// </summary>
    public abstract class PhpComponent : ComponentBase, IDisposable
    {
        protected Context _ctx;

        [Parameter]
        public Context Ctx 
        {
            get => _ctx;
            set { _ctx = value; }
        }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        public IPHPService PhpService { get; set; }

        public virtual void Dispose()
        {
            _ctx?.Dispose();
        }

        protected sealed override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            BuildRenderTree(new PhpTreeBuilder(builder, this));
        }

        protected abstract void BuildRenderTree(PhpTreeBuilder builder);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _ctx ??= BlazorContext.Create(Js, LoggerFactory, PhpService);
        }
    }
}
