using System;
using System.Web;
using System.Web.Mvc;

namespace Bonfire.React.Core
{
    public class ReactHtmlHelper
    {
        public HtmlHelper HtmlHelper { get; set; }

        private static ReactEnvironment Environment
        {
            get { return ReactEnvironment.Current; }
        }

        public ReactHtmlHelper(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }

        /// <summary>
        /// Render the javascript loader needed to add react components
        /// </summary>
        /// <param name="global"></param>
        /// <returns>The compiled javascript code</returns>
        public HtmlString InitLoaderJavascript(string global = null)
        {
            if (!string.IsNullOrEmpty(global))
            {
                ReactConfiguration.setGlobal(global);
            }

            var loaderScript = /*INJECT:JS*/"function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();"/*ENDINJECT*/;

            var script = new TagBuilder("script")
            {
                InnerHtml = string.Format("window.{0}={1}", ReactConfiguration.Global, loaderScript)
            };

            return new HtmlString(script.ToString());
        }

        /// <summary>
        /// Render the components initialization scripts that are waiting to be initialized
        /// </summary>
        /// <returns>The components init javascript</returns>
        public HtmlString InitComponentsJavascript()
        {
            var script = new TagBuilder("script")
            {
                InnerHtml = Environment.GetComponentsInitScript()
            };

            return new HtmlString(script.ToString());
        }

        /// <summary>
        /// Render the specified component
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="props"></param>
        /// <param name="withInit"></param>
        /// <param name="containerId"></param>
        /// <param name="containerClass"></param>
        /// <param name="htmlTag"></param>
        /// <returns>The component Html</returns>
        public HtmlString Render(
            string componentName, 
            object props, 
            bool withInit = false,
            string containerId = null,
            string containerClass = null, 
            string htmlTag = null
        )
        {
            var component = Environment.CreateComponent(componentName, props, withInit, containerId);

            if (!string.IsNullOrEmpty(htmlTag))
            { 
                component.ContainerTag = htmlTag;
            }

            if (!string.IsNullOrEmpty(containerClass))
            {
                component.ContainerClass = containerClass;
            }

            var html = component.RenderHtml();

            if (!withInit)
            {
                return new HtmlString(html);
            }

            var script = new TagBuilder("script")
            {
                InnerHtml = component.RenderJavascript(ReactConfiguration.Global)
            };

            return new HtmlString(html + System.Environment.NewLine + script);
        }

        /// <summary>
        /// Render the specified component wrapped around provided html
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="props"></param>
        /// <param name="withInit"></param>
        /// <param name="containerId"></param>
        /// <param name="containerClass"></param>
        /// <param name="htmlTag"></param>
        /// <returns>The react component wrapped around provided html</returns>
        public ReactPreRenderChunk PreRender(
            string componentName,
            object props,
            bool withInit = false,
            string containerId = null,
            string containerClass = null,
            string htmlTag = null
        )
        {
            var component = Environment.CreateComponent(componentName, props, withInit, containerId);

            if (!string.IsNullOrEmpty(htmlTag))
            {
                component.ContainerTag = htmlTag;
            }

            if (!string.IsNullOrEmpty(containerClass))
            {
                component.ContainerClass = containerClass;
            }

            HtmlHelper.ViewContext.Writer.Write(component.RenderOpeningTag() + System.Environment.NewLine);

            return new ReactPreRenderChunk(this, component, withInit);
        }

        /// <summary>
        /// Renders the closing of the PreRender block
        /// </summary>
        /// <param name="component"></param>
        /// <param name="withInit"></param>
        public void EndPreRender(ReactComponent component, bool withInit)
        {
            if (!withInit)
            {
                HtmlHelper.ViewContext.Writer.Write(component.RenderClosingTag());

                return;
            }

            var script = new TagBuilder("script")
            {
                InnerHtml = component.RenderJavascript(ReactConfiguration.Global)
            };

            HtmlHelper.ViewContext.Writer.Write(component.RenderClosingTag() + System.Environment.NewLine + script);
        }
    }

    public class ReactPreRenderChunk : IDisposable
    {
        private readonly ReactHtmlHelper _helper;
        private readonly ReactComponent _component;
        private readonly bool _withInit;

        public ReactPreRenderChunk(ReactHtmlHelper helper, ReactComponent component, bool withInit)
        {
            _helper = helper;
            _component = component;
            _withInit = withInit;
        }

        public void Dispose()
        {
            _helper.EndPreRender(_component, _withInit);
        }
    }
}
