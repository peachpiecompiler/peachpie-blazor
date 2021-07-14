using Pchp.Core;
using System;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The helper class enables assigning event handler to .NET object from PHP.
    /// </summary>
    public static class EventHelper
    {
        /// <summary>
        /// Adds the event listener without further args specification.
        /// </summary>
        public static void AddSimpleEventListener(Context ctx, object target, string eventName, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, EventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            var eventReflection = target.GetType().GetEvent(eventName);
            eventReflection.AddEventHandler(target, (EventHandler)HandlerDelegate);
        }

        /// <summary>
        /// Adds the event listener with .NET args type specification.
        /// </summary>
        public static void AddEventListener<TEventArgs>(Context ctx, object target, string eventName, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, TEventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            var eventReflection = target.GetType().GetEvent(eventName);
            eventReflection.AddEventHandler(target, (EventHandler<TEventArgs>)HandlerDelegate);
        }
    }
}
