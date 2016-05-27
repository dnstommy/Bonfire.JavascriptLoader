using System;
using System.Web;
using System.Web.Helpers;
using Bonfire.JavascriptLoader.Core;

namespace Bonfire.JavascriptLoader.Components
{
    public class ReactComponent : IJavascriptLoaderComponent
    {
        private object _props;
        private string _stringifiedProps;
        public string ComponentName { get; set; }
        public string ContainerId { get; set; }
        public string ContainerClass { get; set; }
        public string ContainerTag { get; set; }
        public object Props
        {
            get { return _props; }
            set
            {
                _props = value;
                _stringifiedProps = Json.Encode(value);
            }
        }

        public ReactComponent(string name, object props, string id)
        {
            ComponentName = name;
            ContainerId = string.IsNullOrEmpty(id) ? GenerateId() : id;
            ContainerTag = "div";
            Props = props;
        }

        public string RenderHtml()
        {
            return RenderOpeningTag() + RenderClosingTag();
        }

        public string RenderJavascript(string globalVar)
        {
            return string.Format(
                "window.{0}.add('{1}', '{2}', {3});",
                globalVar, 
                ComponentName, 
                ContainerId, 
                new HtmlString(_stringifiedProps)
            );
        }

        private string GenerateId()
        {
            return "react_" + Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", string.Empty)
                .Replace("+", string.Empty)
                .TrimEnd('=');
        }

        public string RenderOpeningTag()
        {
            var attributes = string.Format("id=\"{0}\"", ContainerId);

            if (!string.IsNullOrEmpty(ContainerClass))
            {
                attributes += string.Format(" class=\"{0}\"", ContainerClass);
            }

            return string.Format("<{0} {1}>", ContainerTag, attributes);
        }

        public string RenderClosingTag()
        {
            return string.Format("</{0}>", ContainerTag);
        }
    }
}
