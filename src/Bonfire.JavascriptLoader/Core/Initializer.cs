using System.Web.Mvc;
using JavaScriptEngineSwitcher.Msie.Configuration;
using JavaScriptEngineSwitcher.V8;
using React;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System.Collections.Generic;
using JavaScriptEngineSwitcher.Msie;

namespace Bonfire.JavascriptLoader.Core
{
    public class Initializer
    {
        /// <summary>
        /// The Simple Injector Container
        /// </summary>
        public static Container Container { get; set; }

        /// <summary>
        /// Set the container to have access in areas where DI is not available
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(Container container)
        {
            Container = container;
        }

        /// <summary>
        /// Register our singletons with SimpleInjector
        /// </summary>
        /// <param name="configuration"></param>
        public static void Initialize(JavascriptConfiguration configuration)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<IJavascriptConfiguration>(() => configuration, Lifestyle.Singleton);
            container.Register<IJavascriptFactory, JavascriptFactory>(Lifestyle.Singleton);
            container.Register<IJavascriptEnvironment, JavascriptEnvironment>(Lifestyle.Scoped);
            container.RegisterCollection<FactoryRegistration>(GetAvailableFactories());
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            SetContainer(container);
        }

        /// <summary>
        /// Get available factories to loop through and select
        /// the correct engine for our browser
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<FactoryRegistration> GetAvailableFactories()
        {
            var factories = new List<FactoryRegistration>();

            if (EngineUtility.EnvironmentSupportsClearScript())
            {
                factories.Add(new FactoryRegistration
                {
                    Factory = () => new V8JsEngine(),
                    Priority = 10
                });
            }

            if (EngineUtility.EnvironmentSupportsVroomJs())
            {
                factories.Add(new FactoryRegistration
                {
                    Factory = () => new VroomJsEngine(),
                    Priority = 10
                });
            }

            factories.Add(new FactoryRegistration
            {
                Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraEdgeJsRt }),
                Priority = 20
            });

            factories.Add(new FactoryRegistration
            {
                Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraIeJsRt }),
                Priority = 30
            });

            factories.Add(new FactoryRegistration
            {
                Factory = () => new MsieJsEngine(new MsieConfiguration { EngineMode = JsEngineMode.ChakraActiveScript }),
                Priority = 40
            });

            return factories;
        }
    }
}
