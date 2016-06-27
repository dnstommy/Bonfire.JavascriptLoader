using System.Collections.Generic;
using System.Web;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptConfiguration : IJavascriptConfiguration
    {
        private readonly string _loaderScript = /*INJECT:JS*/"function(){function n(n,r,u){var e={name:n,id:r,props:u};return i?i(e):void t.push(e)}function r(n){i=n(t)}var i,t=[];return{render:n,init:r}}();"/*ENDINJECT:JS*/;

        public bool RenderServerSide { get; private set; }

        public string GlobalJavascriptVar { get; private set; } = "__JavascriptLoader";

        public IList<string> Files { get; set; } = new List<string>();

        public IJavascriptConfiguration SetGlobalJavascriptVar(string global)
        {
            GlobalJavascriptVar = global;

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

        public string GetLoaderScript()
        {
            return _loaderScript;
        }
    }
}
