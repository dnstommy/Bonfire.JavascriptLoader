namespace Bonfire.React.Core
{
    public class ReactConfiguration
    {
        public static string Global { get; private set; } = "__ReactLoader";

        public static void SetGlobal(string global)
        {
            Global = global;
        }
    }
}
