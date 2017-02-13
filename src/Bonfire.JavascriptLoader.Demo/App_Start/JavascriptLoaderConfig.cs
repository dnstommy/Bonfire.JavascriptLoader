﻿using Bonfire.JavascriptLoader.Core;

namespace Bonfire.JavascriptLoader.Demo
{
    public class JavascriptLoaderConfig
    {
        public static void Configure()
        {
            var configuration = new JavascriptConfiguration();

            configuration
                .EnableServerSideRendering()
                .AddFile("/Assets/Vue/client.js");

            Initializer.Initialize(configuration);
        }
    }
}