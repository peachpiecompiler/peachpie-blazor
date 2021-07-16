using System;
using System.IO;
using System.Text;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class writes an input to <see cref="Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder"/> or console depending on its state.
    /// </summary>
    public class BlazorWriter : TextWriter
    {
        private Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder _builder;
        private StringBuilder _buffer;

        #region Create
        private BlazorWriter(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) : base()
        {
            _builder = builder;
            _buffer = new StringBuilder();
        }

        public static BlazorWriter CreateConsole()
        {
            return new BlazorWriter(null);
        }

        public static BlazorWriter CreateTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            return new BlazorWriter(builder);
        }
        #endregion

        #region TextWriter
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string value)
        {
            if (_builder == null)
                Console.Write(value);
            else
                _buffer.Append(value);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _builder = null;
        }

        public override void Flush()
        {
            base.Flush();
            if (_buffer != null && _builder != null && _buffer.Length > 0)
            {
                _builder.AddMarkupContent(0, _buffer.ToString());
                _buffer.Clear();
            }
        }
        #endregion
    }
}
