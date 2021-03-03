﻿using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public static class InteropUtils
    {
        public static void CallJsVoid(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsVoid(method, args);

        public static void CallJsVoidAsync(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsVoid(method, args);

    }

    [PhpType]
    public static partial class GenericHelper
    {
        public static TResult CallJs<TResult>(Context ctx, string method, params object[] args)
        {
            return ((BlazorContext)ctx).CallJs<TResult>(method, args);
        }

        public static ValueTask<TResult> CallJsAsync<TResult>(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsAsync<TResult>(method, args);
    }
}
