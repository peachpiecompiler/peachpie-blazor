using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Pchp.Core;
using System;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class is a wrapper of <see cref="Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder">. It helps to call all Blazor API from PHP.
    /// </summary>
    [PhpType]
    public class PhpTreeBuilder
    {
        public Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder Builder;
        public PhpComponent Component;

        public PhpTreeBuilder(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder, PhpComponent component)
        {
            this.Builder = builder;
            this.Component = component;
        }

        public void OpenElement(int sequence, string elementName) => Builder.OpenElement(sequence, elementName);
        public void CloseElement() => Builder.CloseElement();

#nullable enable
        public void AddAttribute(int sequence, string name, string? value) => Builder.AddAttribute(sequence, name, value);
#nullable disable

        public void AddContent(int sequence, string textContent) => Builder.AddContent(sequence, textContent);

        public void AddMarkupContent(int sequence, string textContent) => Builder.AddMarkupContent(sequence, textContent);

        #region events
        public void AddEventCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create(Component, new Action(() => value.Invoke(ctx))));
        }

        public void AddEventClipboardCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ClipboardEventArgs>(Component, new Action<ClipboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventDragCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<DragEventArgs>(Component, new Action<DragEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventErrorCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ErrorEventArgs>(Component, new Action<ErrorEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventEventCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<EventArgs>(Component, new Action<EventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventFocusCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<FocusEventArgs>(Component, new Action<FocusEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventChangeCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ChangeEventArgs>(Component, new Action<ChangeEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventKeyboardCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<KeyboardEventArgs>(Component, new Action<KeyboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventMouseCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<MouseEventArgs>(Component, new Action<MouseEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventPointerCallback(Context ctx, int sequence, string name, IPhpCallable value)     
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<PointerEventArgs>(Component, new Action<PointerEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventWheelCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<WheelEventArgs>(Component, new Action<WheelEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventProgressCallback(Context ctx, int sequence, string name, IPhpCallable value)    
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ProgressEventArgs>(Component, new Action<ProgressEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventTouchCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            Builder.AddAttribute(sequence, name, EventCallback.Factory.Create<TouchEventArgs>(Component, new Action<TouchEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }
        #endregion
    }

}
