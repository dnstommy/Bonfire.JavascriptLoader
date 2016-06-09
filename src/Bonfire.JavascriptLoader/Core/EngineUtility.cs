using System;

namespace Bonfire.JavascriptLoader.Core
{
    public class EngineUtility
    {
        /// <summary>
        /// Determines if the current environment supports the VroomJs engine.
        /// </summary>
        /// <returns><c>true</c> if VroomJs is supported</returns>
        public static bool EnvironmentSupportsVroomJs()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }

        /// <summary>
        /// Determines if the current environment supports the ClearScript V8 engine
        /// </summary>
        /// <returns><c>true</c> if ClearScript is supported</returns>
        public static bool EnvironmentSupportsClearScript()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }
}
