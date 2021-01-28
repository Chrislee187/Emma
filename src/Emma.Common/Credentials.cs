using System;

namespace Emma.Common
{
    public class Credentials
    {
        public static string AppKey()
        {
            var environmentVariable = Environment.GetEnvironmentVariable("EMMA_APP_KEY");

            return string.IsNullOrEmpty(environmentVariable)
                ? throw new Exception($"Github accesskey not found in environment variable 'EMMA_APP_KEY'")
                : environmentVariable;
        }
    }
}