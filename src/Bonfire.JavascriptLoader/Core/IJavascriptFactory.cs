using JavaScriptEngineSwitcher.Core;

namespace Bonfire.JavascriptLoader.Core
{
    public interface IJavascriptFactory
    {
        IJsEngine GetEngine();

        void ReturnEngineToPool(IJsEngine engine);

        void Dispose();
    }
}
