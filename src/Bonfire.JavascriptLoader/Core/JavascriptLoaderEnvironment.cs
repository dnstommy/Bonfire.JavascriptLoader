using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.Msie.Configuration;
using JavaScriptEngineSwitcher.V8;
using JSPool;
using React;
using ReactComponent = Bonfire.JavascriptLoader.Components.ReactComponent;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptLoaderEnvironment
    {
        protected IList<IJavascriptLoaderComponent> _components = new List<IJavascriptLoaderComponent>();

        protected Lazy<IJsEngine> _engineFromPool;

        protected IList<string> Scripts => new List<string>
        {
            HttpContext.Current.Server.MapPath("/Content/js/server.js")
        };

        protected IEnumerable<Registration> AvailableFactories
        {
            get
            {
                var factories = new List<Registration>();

                if (EnvironmentSupportsClearScript())
                {
                    factories.Add(new Registration
                    {
                        Factory = () => new V8JsEngine(),
                        Priority = 10
                    });
                }

                if (EnvironmentSupportsVroomJs())
                {
                    factories.Add(new Registration
                    {
                        Factory = () => new VroomJsEngine(),
                        Priority = 10
                    });
                }

                factories.Add(new Registration
                {
                    Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraEdgeJsRt }),
                    Priority = 20
                });

                factories.Add(new Registration
                {
                    Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraIeJsRt }),
                    Priority = 30
                });

                factories.Add(new Registration
                {
                    Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraActiveScript }),
                    Priority = 40
                });

                return factories;
            }
        }

        public static JavascriptLoaderEnvironment Current
        {
            get
            {
                return (HttpContext.Current.Items["JavascriptLoaderEnvironment"] ??
                        (HttpContext.Current.Items["JavascriptLoaderEnvironment"] = new JavascriptLoaderEnvironment())) as JavascriptLoaderEnvironment;
            }
        }

        protected virtual IJsEngine Engine
        {
            get { return _engineFromPool.Value; }
        }

        public JavascriptLoaderEnvironment()
        {
            var poolConfig = new JsPoolConfig
            {
                EngineFactory = GetFactory(),
                Initializer = LoadUserScripts
            };

            var pool = new JsPool(poolConfig);

            _engineFromPool = new Lazy<IJsEngine>(() => pool.GetEngine());
        }

        private void LoadUserScripts(IJsEngine engine)
        {
            foreach (var file in Scripts)
            {
                var contents = File.ReadAllText(file, Encoding.UTF8);

                engine.Execute(contents);
            }
        }

        private Func<IJsEngine> GetFactory()
        {
            var availableEngineFactories = AvailableFactories
                .OrderBy(x => x.Priority)
                .Select(x => x.Factory);

            foreach (var engineFactory in availableEngineFactories)
            {
                IJsEngine engine = null;

                try
                {
                    engine = engineFactory();

                    if (EngineIsUsable(engine, true))
                    {
                        return engineFactory;
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("Error initialising {0}: {1}", engineFactory, ex));
                }
                finally
                {
                    engine?.Dispose();
                }
            }
            
            throw new Exception("Someting wong!");
        }

        /// <summary>
        /// Performs a sanity check to ensure the specified engine type is usable.
        /// </summary>
        /// <param name="engine">Engine to test</param>
        /// <param name="allowMsie">Whether the MSIE engine can be used</param>
        /// <returns></returns>
        private static bool EngineIsUsable(IJsEngine engine, bool allowMsie)
        {
            var isUsable = engine.Evaluate<int>("1 + 1") == 2;
            var isMsie = engine is MsieJsEngine;

            return isUsable && (!isMsie || allowMsie);
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
            const string loaderScript = "function(){function n(n,r,t){var u={name:n,id:r,props:t};return e?e(u):void i.push(u)}function r(n){e=n,i.reverse().reduceRight(function(n,r,t){return e(r),i.splice(t,1),r},{})}var e,i=[];return{add:n,init:r}}();";

            var script = new TagBuilder("script")
            {
                InnerHtml = string.Format("window.{0}={1}", JavascriptLoaderConfiguration.Global, loaderScript)
            };

            return script.ToString();
        }

        public virtual T Execute<T>(string code)
        {
            return Engine.Evaluate<T>(code);
        }

        /// <summary>
        /// Determines if the current environment supports the VroomJs engine.
        /// </summary>
        /// <returns><c>true</c> if VroomJs is supported</returns>
        public static bool EnvironmentSupportsVroomJs()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        /// <summary>
        /// Determines if the current environment supports the ClearScript V8 engine
        /// </summary>
        /// <returns><c>true</c> if ClearScript is supported</returns>
        public static bool EnvironmentSupportsClearScript()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }

    public class Registration
    {
        /// <summary>
        /// Gets or sets the factory for this JavaScript engine
        /// </summary>
        public Func<IJsEngine> Factory { get; set; }

        /// <summary>
        /// Gets or sets the priority for this JavaScript engine. Engines with lower priority
        /// are preferred.
        /// </summary>
        public int Priority { get; set; }
    }
}
