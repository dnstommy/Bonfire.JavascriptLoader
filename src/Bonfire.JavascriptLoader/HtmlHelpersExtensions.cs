using System.Web.Mvc;
using Bonfire.JavascriptLoader.Core;

namespace Bonfire.JavascriptLoader
{
    public static class HtmlHelperExtensions
    {
        public static JavascriptHtmlHelper Javascript(this HtmlHelper html)
        {
            return new JavascriptHtmlHelper(
                Initializer.Container.GetInstance<IJavascriptEnvironment>(),
                Initializer.Container.GetInstance<IJavascriptConfiguration>()
            );
        }
    }
}