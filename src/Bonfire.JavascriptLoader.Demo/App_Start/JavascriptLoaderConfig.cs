using Bonfire.JavascriptLoader.Core;

namespace Bonfire.JavascriptLoader.Demo
{
    public class JavascriptLoaderConfig
    {
        public static void Configure()
        {
            var configuration = new JavascriptConfiguration();

            configuration
                .EnableServerSideRendering()
                .AddFile("/Content/js/server.js");

            Initializer.Initialize(configuration);
        }
    }
}