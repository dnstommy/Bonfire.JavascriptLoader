using System;
using JavaScriptEngineSwitcher.Core;

namespace Bonfire.JavascriptLoader.Core
{
    public class FactoryRegistration
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
