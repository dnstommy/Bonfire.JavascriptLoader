namespace Bonfire.JavascriptLoader.Core
{
    public class JavascriptLoaderConfiguration
    {
        public static string Global { get; private set; } = "__JavascriptLoader";

        public static void SetGlobal(string global)
        {
            Global = global;
        }
    }
}
