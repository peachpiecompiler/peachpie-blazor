using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Peachpie.Blazor
{

    /// <summary>
    /// The interface defines a rendering API used in Blazor.
    /// </summary>
    [PhpType]
    public interface BlazorWritable
    {
        /// <summary>
        /// Writes the content to <see cref="PhpTreeBuilder"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="BlazorContext"/>.</param>
        /// <param name="startIndex">The next sequence number used by Blazor diff algorithm.</param>
        /// <returns>Returns the next sequence number after the written content.</returns>
        public int writeWithTreeBuilder(Context ctx, PhpTreeBuilder builder, int startIndex);
    }
    
    /// <summary>
    /// The class represents a text content of web page.
    /// </summary>
    [PhpType]
    public class Text : BlazorWritable
    {
        protected string content;

        public Text(string content)
        {
            this.content = content;
        }

        #region Conversions
        public static implicit operator string(Text text) => text.content;
        public static implicit operator Text(string text) => new Text(text);
        #endregion

        #region iBlazorWritable
        public int writeWithTreeBuilder(Context ctx, PhpTreeBuilder builder, int startIndex)
        {
            builder.AddContent(startIndex++, content);
            return startIndex;
        }
        #endregion

        public override string ToString()
        {
            return content;
        }
    }
    
    /// <summary>
    /// The class represents an HTML tag.
    /// </summary>
    [PhpType]
    public class Tag : BlazorWritable
    {
        /// <summary>
        /// The name of tag.
        /// </summary>
        public string name;
        /// <summary>
        /// Tag attributes
        /// </summary>
        public AttributeCollection attributes;
        /// <summary>
        /// Tag content represented as a collection of children.
        /// </summary>
        public List<BlazorWritable> content;

        public Tag():this("div")
        { }

        public Tag(string name)
        {
            this.name = name;
            attributes = new AttributeCollection();
            content = new List<BlazorWritable>();
        }

        public void __construct(string name)
        {
            this.name = name;
        }

        #region BlazorWritable
        public int writeWithTreeBuilder(Context ctx, PhpTreeBuilder builder, int startIndex)
        {
            builder.OpenElement(startIndex++, name);

            startIndex = attributes.writeWithTreeBuilder(ctx, builder, startIndex);

            content.ForEach(x => startIndex = x.writeWithTreeBuilder(ctx, builder, startIndex));

            builder.CloseElement();
            return startIndex;
        }
        #endregion

        public override string ToString()
        {
            string result = $"<{name} {attributes.ToString()}>";
            content.ForEach((item) => result += item.ToString());
            result += $"</{name}>";
            return result;
        }
    }
    
    /// <summary>
    /// The class represents attributes of tag.
    /// </summary>
    [PhpType]
    public class AttributeCollection : BlazorWritable, ArrayAccess
    {
        protected PhpArray attributes;
        protected Dictionary<string, IPhpCallable> events;
        protected CssBuilder styles;
        protected ClassBuilder classes;

        public AttributeCollection()
        {
            attributes = new PhpArray();
            events = new Dictionary<string, IPhpCallable>();
        }

        /// <summary>
        /// Adds the event to <see cref="PhpTreeBuilder"/>. It passes the current sequence number and the builder to the accepted handler.   
        /// </summary>
        public void addEvent(string name, IPhpCallable handler)
        {
            events.Add(name, handler);
        }

        /// <summary>
        /// Removes the event by name.   
        /// </summary>
        public void removeEvent(string name)
        {
            if (events.ContainsKey(name))
                events.Remove(name);
        }

        #region iBlazorWritable
        public int writeWithTreeBuilder(Context ctx, PhpTreeBuilder builder, int startIndex)
        {
            foreach (var item in attributes)
            {
                if (!item.Key.IsString)
                    builder.AddAttribute(startIndex++, item.Value.ToString(), null);
                else
                    builder.AddAttribute(startIndex++, item.Key.String, item.Value.ToString());
            }

            if (styles != null)
                builder.AddAttribute(startIndex++, "style", styles.ToString());

            if (classes != null)
                builder.AddAttribute(startIndex++, "class", classes.ToString());

            foreach (var item in events)
            {
                item.Value.Invoke(ctx, startIndex++, PhpValue.FromClass(builder));
            }

            return startIndex;
        }
        #endregion

        #region ArrayAccess
        public PhpValue offsetGet(PhpValue offset)
        {
            if (attributes.ContainsKey(offset))
                return attributes[offset].AsPhpAlias();
            else if (offset.AsString() == "style")
            {
                styles ??= new CssBuilder();
                return PhpValue.FromClass(styles).AsPhpAlias();
            }
            else if (offset.AsString() == "class")
            {
                classes ??= new ClassBuilder();
                return PhpValue.FromClass(classes).AsPhpAlias();
            }
            else
                throw new ArgumentException();
        }

        public void offsetSet(PhpValue offset, PhpValue value)
        {
            attributes[offset] = value;
        }

        public void offsetUnset(PhpValue offset)
        {
            attributes.Remove(offset);
        }

        public bool offsetExists(PhpValue offset)
        {
            return attributes.ContainsKey(offset);
        }
        #endregion

        public override string ToString()
        {
            string result = "";

            foreach (var item in attributes)
            {
                if (!item.Key.IsString)
                    result += item.Value.ToString() + " ";
                else
                    result += $"{item.Key.String}=\"{item.Value.ToString()}\" ";
            }

            if (styles != null)
                result += $"style=\"{styles.ToString()}\" ";

            if (classes != null)
                result += $"class=\"{classes.ToString()}\" ";

            return result;
        }
    }
    
    /// <summary>
    /// The class formats css styles into an HTML string(key1:value1;key2:value2).
    /// </summary>
    [PhpType]
    public class CssBuilder : ArrayAccess
    {
        protected Dictionary<string, string> _styles;

        public CssBuilder()
        {
            _styles = new Dictionary<string, string>();
        }

        #region ArrayAccess
        public bool offsetExists(PhpValue offset)
        {
            return _styles.ContainsKey(offset.AsString());
        }

        public PhpValue offsetGet(PhpValue offset)
        {
            if (offsetExists(offset))
                return _styles[offset.AsString()];
            else
                return PhpValue.False;
        }

        public void offsetSet(PhpValue offset, PhpValue value)
        {
            _styles[offset.AsString()] = value.AsString();
        }

        public void offsetUnset(PhpValue offset)
        {
            _styles.Remove(offset.AsString());
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _styles)
            {
                sb.Append(item.Key);
                sb.Append(":");
                sb.Append(item.Value);
                sb.Append(";");
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// The class formats tag classes into an HTML string(class1 class2).
    /// </summary>
    [PhpType]
    public class ClassBuilder
    {
        protected List<string> _classes;

        public ClassBuilder()
        {
            _classes = new List<string>();
        }

        /// <summary>
        /// Adds the class name to the collection.
        /// </summary>
        public void add(string @class) => _classes.Add(@class);

        /// <summary>
        /// Removes the class name to the collection.
        /// </summary>
        public void remove(string @class)
        {
            _classes.Remove(@class);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _classes)
            {
                sb.Append(item);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Wrapper of .NET timer.
    /// </summary>
    [PhpType]
    public class Timer : IDisposable
    {
        public System.Timers.Timer timer;

        public Timer(double interval)
        {
            timer = new System.Timers.Timer(interval);
        }

        /// <summary>
        /// Adds PHP handler to the event.
        /// </summary>
        public void addEventElapsed(Context ctx, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, ElapsedEventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandlerDelegate);
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        /// <summary>
        /// Timer raises the Elapsed event only once, when it is set to false.
        /// </summary>
        public void AutoReset(bool indicator)
        {
            timer.AutoReset = indicator;
        }

        /// <summary>
        /// Timer starts to raise events.
        /// </summary>
        public void Start() => timer.Start();

        
        /// <summary>
        /// Timer stops to raise events.
        /// </summary>
        public void Stop() => timer.Stop();

    }
}
