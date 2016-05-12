using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Bonfire.React.Core
{
    public class ReactEnvironment
    {
        protected IList<ReactComponent> _components = new List<ReactComponent>();

        public static ReactEnvironment Current
        {
            get
            {
                return (HttpContext.Current.Items["ReactEnvironment"] ??
                        (HttpContext.Current.Items["ReactEnvironment"] = new ReactEnvironment())) as ReactEnvironment;
            }
        }

        public ReactComponent CreateComponent(string name, object props, bool withInit, string id)
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
                script.Append(component.RenderJavascript(ReactConfiguration.Global));
                script.AppendLine(";");
            }

            return script.ToString();
        }
    }
}
