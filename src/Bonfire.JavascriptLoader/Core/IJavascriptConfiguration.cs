using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonfire.JavascriptLoader.Core
{
    public interface IJavascriptConfiguration
    {
        bool RenderServerSide { get; }

        string GlobalJavascriptVar { get; }

        IList<string> Files { get; }

        IJavascriptConfiguration SetGlobalJavascriptVar(string global);

        IJavascriptConfiguration AddFile(string file);

        IJavascriptConfiguration EnableServerSideRendering(bool enable);

        string GetLoaderScript();
    }
}
