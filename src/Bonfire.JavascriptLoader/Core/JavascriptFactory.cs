using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;
using JSPool;

namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptFactory : IJavascriptFactory
    {
        /// <summary>
        /// Configuration instance
        /// </summary>
        private readonly IJavascriptConfiguration _configuration;

        /// <summary>
        /// The js engine factory instance
        /// </summary>
        private readonly Func<IJsEngine> _factory;

        /// <summary>
        /// The js pool instance
        /// </summary>
        private JsPool _pool;

        /// <summary>
        /// Whether or not the engine has been disposed
        /// </summary>
        private bool _disposed;

        private Exception _scriptLoadException;

        /// <summary>
        /// Initialize the JSPool
        /// </summary>
        /// <param name="availableFactories"></param>
        /// <param name="configuration"></param>
        public JavascriptFactory(IEnumerable<FactoryRegistration> availableFactories, IJavascriptConfiguration configuration)
        {
            if (!configuration.RenderServerSide) return;

            _configuration = configuration;
            _factory = GetFactory(availableFactories);
            _pool = CreatePool();
        }

        /// <summary>
        /// Create a new js Pool
        /// </summary>
        /// <returns></returns>
        private JsPool CreatePool()
        {
            var config = new JsPoolConfig
            {
                EngineFactory = _factory,
                Initializer = LoadScripts
            };

            var pool = new JsPool(config);

            // Reset the recycle exception on recycle. If there *are* errors loading the scripts
            // during recycle, the errors will be caught in the initializer.
            pool.Recycled += null;

            return pool;
        }

        /// <summary>
        /// Gets the current engine for pool
        /// </summary>
        /// <returns></returns>
        public IJsEngine GetEngine()
        {
            if (_scriptLoadException != null)
            {
                // This means an exception occurred while loading the script (eg. syntax error in the file)
                throw _scriptLoadException;
            }

            return _pool.GetEngine();
        }

        /// <summary>
        /// Load the user scripts provided
        /// </summary>
        /// <param name="engine"></param>
        private void LoadScripts(IJsEngine engine)
        {
            engine.Execute(string.Format("{0}={1}", _configuration.GlobalJavascriptVar, _configuration.GetLoaderScript()));
            engine.Execute(string.Format("{0}.isServerSide=true;", _configuration.GlobalJavascriptVar));

            foreach (var file in _configuration.Files)
            {
                try
                {
                    var contents = File.ReadAllText(file, Encoding.UTF8);

                    engine.Execute(contents);
                }
                catch (JsRuntimeException ex)
                {
                    // We can't simply rethrow the exception here, as it's possible this is running
                    // on a background thread (ie. as a response to a file changing). If we did
                    // throw the exception here, it would terminate the entire process. Instead,
                    // save the exception, and then just rethrow it later when getting the engine.
                    _scriptLoadException = new Exception(string.Format(
                        "Error while loading \"{0}\": {1}\r\nLine: {2}\r\nColumn: {3}",
                        file,
                        ex.Message,
                        ex.LineNumber,
                        ex.ColumnNumber
                    ));
                }

            }
        }

        /// <summary>
        /// Get the js engine to use
        /// </summary>
        /// <returns></returns>
        private static Func<IJsEngine> GetFactory(IEnumerable<FactoryRegistration> availableFactories)
        {
            var availableEngineFactories = availableFactories
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

            throw new Exception("No engine factory could be determined.");
        }

        /// <summary>
        /// Returns an engine to the pool so it can be reused
        /// </summary>
        /// <param name="engine">Engine to return</param>
        public virtual void ReturnEngineToPool(IJsEngine engine)
        {
            // This could be called from Environment.Dispose if that class is disposed after 
            // this class. Let's just ignore this if it's disposed.
            if (!_disposed)
            {
                _pool.ReturnEngineToPool(engine);
            }
        }

        /// <summary>
        /// Clean up all engines
        /// </summary>
        public virtual void Dispose()
        {
            _disposed = true;

            if (_pool == null) return;

            _pool.Dispose();
            _pool = null;
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
    }
}
