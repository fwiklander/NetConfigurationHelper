namespace Wiksoft.NetConfigurationHelper
{
    public interface IConfigurationHelper
    {
        /// <summary>
        /// Get a configuration value from AppSettings or ConnectionStrings
        /// If the key exists in both sections the AppSettings key is chosen
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <returns>The configuration value</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the key is not found in any configuration section</exception>
        /// <exception cref="System.FormatException">Thrown when the value can not be converted to the specified type</exception>
        T GetConfiguration<T>(string key);

        /// <summary>
        /// Get a configuration value from AppSettings or the specified default if it does not exist
        /// </summary>
        /// <param name="key">The configuration key</param>
        /// <param name="defaultValue">Default value to return if configuration does not exist.
        /// If no default is specified default(T) is used instead</param>
        /// <returns>The configuration value or a specified default value</returns>
        /// <exception cref="System.FormatException">Thrown when the value can not be converted to the specified type</exception>
        T GetConfigurationOrDefault<T>(string key, T defaultValue = default(T));
    }
}