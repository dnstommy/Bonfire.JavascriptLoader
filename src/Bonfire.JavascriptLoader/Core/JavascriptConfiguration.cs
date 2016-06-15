using System.Collections.Generic;
using System.Web;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptConfiguration : IJavascriptConfiguration
    {
        public bool RenderServerSide { get; private set; }

        public string ClientGlobal { get; private set; } = "__JavascriptLoader";

        public string ServerGlobal { get; private set; } = "___JavascriptLoader";

        public IList<string> Files { get; set; } = new List<string>();

        public IJavascriptConfiguration SetClientGlobal(string global)
        {
            ClientGlobal = global;

            return this;
        }

        public IJavascriptConfiguration SetServerGlobal(string global)
        {
            ServerGlobal = global;

            return this;
        }

        public IJavascriptConfiguration AddFile(string file)
        {
            var mappedPath = HttpContext.Current.Server.MapPath(file);

            Files.Add(mappedPath);

            return this;
        }

        public IJavascriptConfiguration EnableServerSideRendering(bool enable = true)
        {
            RenderServerSide = enable;

            return this;
        }
    }
}
