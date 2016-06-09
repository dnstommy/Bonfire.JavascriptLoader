namespace Bonfire.JavascriptLoader.Core
{
    public interface IJavascriptEnvironment
    {
        IJavascriptComponent CreateComponent(string name, object props, bool withInit, string id, bool renderServerSide);

        string GetComponentsInitScript();

        string GetLoaderScript();

        void AddConsoleCall(string type, string message);

        T Execute<T>(string code);

        void Dispose();

        void ReturnEngineToPool();
    }
}
