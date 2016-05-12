using System.Web.Mvc;
using Bonfire.React.Core;

namespace Bonfire.React
{
    public static class HtmlHelperExtensions
    {
        public static ReactHtmlHelper React(this HtmlHelper html)
        {
            return new ReactHtmlHelper(html);
        }
    }
}