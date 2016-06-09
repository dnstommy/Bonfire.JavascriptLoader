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

        string ClientGlobal { get; }

        string ServerGlobal { get; }

        IList<string> Files { get; }

        IJavascriptConfiguration SetClientGlobal(string global);

        IJavascriptConfiguration SetServerGlobal(string global);

        IJavascriptConfiguration AddFile(string file);

        IJavascriptConfiguration EnableServerSideRendering(bool enable);
    }
}
