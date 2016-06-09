using System;
using System.Collections.Generic;
using System.Text;
using JavaScriptEngineSwitcher.Core;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptEnvironment : IJavascriptEnvironment
    {
        private readonly IList<IJavascriptComponent> _components = new List<IJavascriptComponent>();

        private IJavascriptFactory _engineFactory { get; }

        private IJavascriptConfiguration _configuration { get; }

        private Lazy<IJsEngine> _engineFromPool;

        private IList<JavascriptConsole> _consoles { get; } = new List<JavascriptConsole>();

        protected virtual IJsEngine Engine => _engineFromPool.Value;

        public JavascriptEnvironment(IJavascriptConfiguration configuration, IJavascriptFactory factory)
        {
            _configuration = configuration;
            _engineFactory = factory;
            _engineFromPool = new Lazy<IJsEngine>(() => _engineFactory.GetEngine());
        }

        public IJavascriptComponent CreateComponent(string name, object props, bool withInit, string id, bool renderServerSide)
        {
            var component = new JavascriptComponent(this, name, props, id, (_configuration.RenderServerSide && renderServerSide));

            if (!withInit)
            {
                _components.Add(component);
            }

            return component;
        }

        public string GetComponentsInitScript()
        {
            var script = new StringBuilder();

            foreach (var console in _consoles)
            {
                script.Append(console.GetScript());
            }

            foreach (var component in _components)
            {
                script.Append(component.RenderJavascript(_configuration.ClientGlobal));
            }

            return script.ToString();
        }

        public string GetLoaderScript()
        {
            const string loaderScript = "function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();";

            return string.Format("window.{0}={1}", _configuration.ClientGlobal, loaderScript);
        }

        public virtual T Execute<T>(string code)
        {
            return Engine.Evaluate<T>(code);
        }

        public void AddConsoleCall(string type, string message)
        {
            _consoles.Add(new JavascriptConsole
            {
                Type = type,
                Message = message
            });
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            ReturnEngineToPool();
        }

        /// <summary>
        /// Returns the currently held JS engine to the pool. (no-op if engine pooling is disabled)
        /// </summary>
        public void ReturnEngineToPool()
        {
            if (_engineFromPool.IsValueCreated)
            {
                _engineFactory.ReturnEngineToPool(_engineFromPool.Value);
                _engineFromPool = new Lazy<IJsEngine>(() => _engineFactory.GetEngine());
            }
        }
    }

    public class JavascriptConsole
    {
        public string Type { get; set; }

        public string Message { get; set; }

        public string GetScript()
        {
            return string.Format("console.{0}('{1}');", Type, Message.Replace("'", @"\'"));
        }
    }
}
