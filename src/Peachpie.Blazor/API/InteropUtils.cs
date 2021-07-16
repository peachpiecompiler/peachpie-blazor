using Pchp.Core;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The helper class enables to call Javascript functions from PHP.
    /// </summary>
    public static class InteropUtils
    {
        /// <summary>
        /// Call a Javascript void function by given name and parameters.
        /// </summary>
        public static void CallJsVoid(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsVoid(method, args);

        /// <summary>
        /// Call a Javascript void function by given name and parameters asynchronyously.
        /// </summary>
        public static void CallJsVoidAsync(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsVoidAsync(method, args);

    }

    /// <summary>
    /// The helper class enables to call generic functions from PHP.
    /// </summary>
    [PhpType]
    public static partial class GenericHelper
    {
        /// <summary>
        /// Call a Javascript function by given name and parameters.
        /// </summary>
        public static TResult CallJs<TResult>(Context ctx, string method, params object[] args)
        {
            return ((BlazorContext)ctx).CallJs<TResult>(method, args);
        }

        /// <summary>
        /// Call a Javascript function by given name and parameters asynchronyously.
        /// </summary>
        public static ValueTask<TResult> CallJsAsync<TResult>(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsAsync<TResult>(method, args);
    }
}
