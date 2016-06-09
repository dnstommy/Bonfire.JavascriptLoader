namespace Bonfire.JavascriptLoader.Core
{
    public interface IJavascriptComponent
    {
        string ComponentName { get; set; }

        string ContainerId { get; set; }

        string ContainerClass { get; set; }

        string ContainerTag { get; set; }

        bool RenderServerSide { get; set; }

        object Props { get; set; }

        string RenderHtml(string clientGlobal);

        string RenderJavascript(string serverGlobal);
    }
}
