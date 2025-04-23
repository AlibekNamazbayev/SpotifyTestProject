using System;

namespace SpotifyTestProject.Core.Utils
{
    public static class EnvironmentHelper
    {
        public static string GetRequiredVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"{name} environment variable is not set");
            return value;
        }

        public static string GetVariableOrDefault(string name, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(name);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}
