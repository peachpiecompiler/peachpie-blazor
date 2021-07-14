using Pchp.Core;
using System;
using System.IO;

namespace Desktop
{
    public class Program
    {
        const string ScriptPath = "index.php";

        static void Main(string[] args)
        {
            Context.AddScriptReference(typeof(force).Assembly);

            using (var ctx = Context.CreateConsole(string.Empty, args))
            {
                
                var script = Context.TryGetDeclaredScript(ScriptPath);

                script.Evaluate(ctx, ctx.Globals, null);
            }
        }
    }
}
