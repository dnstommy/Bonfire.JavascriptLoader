using Bonfire.JavascriptLoader.Components;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptLoaderEnvironment
    {
        protected IList<IJavascriptLoaderComponent> _components = new List<IJavascriptLoaderComponent>();

        public static JavascriptLoaderEnvironment Current
        {
            get
            {
                return (HttpContext.Current.Items["JavascriptLoaderEnvironment"] ??
                        (HttpContext.Current.Items["JavascriptLoaderEnvironment"] = new JavascriptLoaderEnvironment())) as JavascriptLoaderEnvironment;
            }
        }

        public IJavascriptLoaderComponent CreateComponent(string name, object props, bool withInit, string id)
        {
            var component = new ReactComponent(name, props, id);

            if (!withInit)
            {
                _components.Add(component);
            }

            return component;
        }

        public string GetComponentsInitScript()
        {
            var script = new StringBuilder();

            foreach (var component in _components)
            {
                script.Append(component.RenderJavascript(JavascriptLoaderConfiguration.Global));
                script.AppendLine(";");
            }

            return script.ToString();
        }
    }
}
