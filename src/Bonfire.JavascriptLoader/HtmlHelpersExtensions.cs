using System.Web.Mvc;
using Bonfire.JavascriptLoader.Core;

namespace Bonfire.JavascriptLoader
{
    public static class HtmlHelperExtensions
    {
        public static JavascriptLoaderHtmlHelper Javascript(this HtmlHelper html)
        {
            return new JavascriptLoaderHtmlHelper(html);
        }
    }
}