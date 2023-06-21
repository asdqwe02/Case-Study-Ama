using System;

namespace GFramework.Utils
{
    public class CommandLineArgumentsHelper
    {
        private static string[] _cachedStartupArguments;

        public static string GetArguments(string parameter)
        {
            _cachedStartupArguments ??= Environment.GetCommandLineArgs();
            string s = null;
            for(var i = 0; i < _cachedStartupArguments.Length; i++)
            {
                if(_cachedStartupArguments[i].Equals(parameter) && _cachedStartupArguments.Length >= i+2)
                {
                    s = _cachedStartupArguments[i+1];
                }
            }
            return s;
        }
    }
}