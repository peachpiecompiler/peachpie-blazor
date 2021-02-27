﻿using Microsoft.AspNetCore.Components;
using Pchp.Core;

[assembly: PhpExtension]

namespace PhpBlazor
{
    public abstract class PhpComponent : ComponentBase
    {
        protected sealed override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            BuildRenderTree(new RenderTreeBuilder(builder, this));
        }

        protected abstract void BuildRenderTree(RenderTreeBuilder builder);
    }
}