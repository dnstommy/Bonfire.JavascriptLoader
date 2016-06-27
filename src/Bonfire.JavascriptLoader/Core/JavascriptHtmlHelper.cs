using System.Web;
using System.Web.Mvc;

namespace Bonfire.JavascriptLoader.Core
{
	public class JavascriptHtmlHelper
	{
		private readonly IJavascriptEnvironment _environment;

		private readonly IJavascriptConfiguration _configuration;

		public JavascriptHtmlHelper(IJavascriptEnvironment environment, IJavascriptConfiguration configuration)
		{
			_environment = environment;
			_configuration = configuration;
		}

		/// <summary>
		/// Render the javascript loader needed to add react components
		/// </summary>
		/// <returns>The compiled javascript code</returns>
		public HtmlString Loader()
		{
			var script = new TagBuilder("script")
			{
				InnerHtml = _environment.GetLoaderScript()
			};

			return new HtmlString(script.ToString());
		}

		/// <summary>
		/// Render the components initialization scripts that are waiting to be initialized
		/// </summary>
		/// <returns>The components init javascript</returns>
		public HtmlString Components()
		{
			var script = new TagBuilder("script")
			{
				InnerHtml = _environment.GetComponentsInitScript()
			};

			return new HtmlString(script.ToString());
		}

		/// <summary>
		/// Render the specified component
		/// </summary>
		/// <param name="componentName"></param>
		/// <param name="props"></param>
		/// <param name="withInit"></param>
		/// <param name="containerId"></param>
		/// <param name="containerClass"></param>
		/// <param name="htmlTag"></param>
		/// <param name="renderServerSide"></param>
		/// <returns>The component Html</returns>
		public HtmlString Render(
			string componentName,
			object props,
			bool withInit = false,
			string containerId = null,
			string containerClass = null,
			string htmlTag = null,
			bool renderServerSide = true
			)
		{
			try
			{
				var component = _environment.CreateComponent(componentName, props, withInit, containerId, renderServerSide);

				if (!string.IsNullOrEmpty(htmlTag))
				{
					component.ContainerTag = htmlTag;
				}

				if (!string.IsNullOrEmpty(containerClass))
				{
					component.ContainerClass = containerClass;
				}

				var html = component.RenderHtml(_configuration.GlobalJavascriptVar);

				if (!withInit)
				{
					return new HtmlString(html);
				}

				var script = new TagBuilder("script")
				{
					InnerHtml = component.RenderJavascript(_configuration.GlobalJavascriptVar)
				};

				return new HtmlString(html + System.Environment.NewLine + script);

			}
			finally
			{
				_environment.ReturnEngineToPool();
			}
		}
	}
}
