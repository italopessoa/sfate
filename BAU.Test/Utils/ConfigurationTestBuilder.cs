using System;
using Microsoft.Extensions.Configuration;

namespace BAU.Test.Utils
{
    /// <summary>
    /// Configutation test class
    /// </summary>
    public static class ConfigurationTestBuilder
    {
        /// <summary>
        /// Configurations
        /// </summary>
        public static IConfiguration GetConfiguration(string name) =>
         new ConfigurationBuilder().AddJsonFile($"appsettings.Test.{ (String.IsNullOrEmpty(name) ? "" : name + ".") }json").Build();

        public static IConfiguration GetConfiguration() => GetConfiguration(String.Empty);
    }
}
