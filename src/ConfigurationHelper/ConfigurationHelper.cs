using System;
using System.ComponentModel;
using System.Configuration;

namespace Wiksoft.NetConfigurationHelper
{
    public sealed class ConfigurationHelper : IConfigurationHelper
    {
        T IConfigurationHelper.GetConfiguration<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            var connString = ConfigurationManager.ConnectionStrings[key];
            if (appSetting == null && connString == null)
            {
                throw new InvalidOperationException($"No configuration key matching {key} was found!");
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                if (appSetting != null)
                {
                    return (T) converter.ConvertFromString(appSetting);
                }

                return (T) converter.ConvertFromString(connString.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to convert value '{appSetting ?? connString.ConnectionString}' found under key {key} to type {typeof(T)}.", ex);
            }
        }

        T IConfigurationHelper.GetConfigurationOrDefault<T>(string key, T defaultValue)
        {
            try
            {
                return (this as IConfigurationHelper).GetConfiguration<T>(key);
            }
            catch (InvalidOperationException)
            {
                return defaultValue;
            }
        }
    }
}
