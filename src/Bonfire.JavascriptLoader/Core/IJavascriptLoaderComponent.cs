namespace Bonfire.JavascriptLoader.Core
{
    public interface IJavascriptLoaderComponent
    {
        string ComponentName { get; set; }
        string ContainerId { get; set; }
        string ContainerClass { get; set; }
        string ContainerTag { get; set; }
        object Props { get; set; }
        string RenderJavascript(string globalVar);
        string RenderHtml();
        string RenderOpeningTag();
        string RenderClosingTag();
    }
}
