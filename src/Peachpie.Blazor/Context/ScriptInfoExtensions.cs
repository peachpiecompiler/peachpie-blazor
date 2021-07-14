using Pchp.Core;

namespace Peachpie.Blazor
{
    [PhpHidden]
    public static class ScriptInfoExtensions
    {
        /// <summary>
        /// Evaluates the script with given Blazor context and render tree builder.
        /// </summary>
        public static PhpValue Evaluate(this Context.ScriptInfo script, BlazorContext ctx, Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            ctx.StartRender(builder);
            var result = script.Evaluate(ctx, ctx.Globals, null);
            ctx.StopRender();

            return result;
        }
    }
}
