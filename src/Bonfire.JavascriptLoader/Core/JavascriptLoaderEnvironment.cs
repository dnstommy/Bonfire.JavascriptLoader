using System;
using Bonfire.JavascriptLoader.Components;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

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
            var component = new ReactComponent(this, name, props, id);

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

        public string GetLoaderScript()
        {
            var loaderScript = /*INJECT:JS*/"function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();"/*ENDINJECT*/;
            var script = new TagBuilder("script")
            {
                InnerHtml = string.Format("window.{0}={1}", JavascriptLoaderConfiguration.Global, loaderScript)
            };

            return script.ToString();
        }

        public virtual string Execute<T>(string code)
        {
            // Take Loader Script => GetLoaderScript();
            // Take Server Script => Server.js;
            // Take Code => code
            // Evaluate It and return it
            return "";
        }
    }
}
