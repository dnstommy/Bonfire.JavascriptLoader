namespace Bonfire.React.Core
{
    public class ReactConfiguration
    {
        private static string _global = "__ReactLoader";

        public static string Global
        {
            get { return _global; }
        }

        public static void setGlobal(string global)
        {
            _global = global;
        }
    }
}
