using System;
using System.Configuration;
using NUnit.Framework;
using Wiksoft.NetConfigurationHelper;

namespace Wiksoft.ConfigurationHelper.IntegrationTest
{
    public class ConfigurationHelperTests
    {
        private IConfigurationHelper _configHelper;
        private const string KeyForString = "StringValue";
        private const string KeyForInt = "IntValue";
        private const string KeyForBoolTrue = "BoolValueTrue";
        private const string KeyForConn = "Connection";

        [SetUp]
        public void TestSetup()
        {
            _configHelper = new NetConfigurationHelper.ConfigurationHelper();
        }

        #region AppSettings

        [Test]
        public void GetConfiguration_KeyDoesNotExist_ExceptionThrown()
        {
            const string Key = "SomeRandomKey";
            var exception = Assert.Throws<InvalidOperationException>(() => _configHelper.GetConfiguration<string>(Key));
            Assert.That(exception.Message, Does.Contain(Key));
        }

        [Test]
        public void GetConfiguration_KeyExistsAsAppSetting_ValueReturned()
        {
            var actual = GetAppSettingValue(KeyForString);
            var value = _configHelper.GetConfiguration<string>(KeyForString);
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(actual));
        }

        [Test]
        public void GetConfiguration_AppSettingIsInt_ValueReturnedAsInt()
        {
            var actual = int.Parse(GetAppSettingValue(KeyForInt));
            var value = _configHelper.GetConfiguration<int>(KeyForInt);
            Assert.That(value, Is.Not.Null);
            Assert.That(value, Is.EqualTo(actual));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueExistsAsString_ConfigValueReturned()
        {
            var actual = GetAppSettingValue(KeyForString);
            var value = _configHelper.GetConfigurationOrDefault(KeyForString, "Random");
            Assert.That(value, Is.EqualTo(actual));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueDoesNotExist_DefaultValueReturned()
        {
            const string DefaultValue = "DefaultString";
            var value = _configHelper.GetConfigurationOrDefault("NoKey", DefaultValue);
            Assert.That(value, Is.EqualTo(DefaultValue));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueExists_ValueReturned()
        {
            var expected = GetAppSettingValue(KeyForString);
            var value = _configHelper.GetConfigurationOrDefault<string>(KeyForString);
            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueDoesNotExist_StringDefaultReturned()
        {
            var value = _configHelper.GetConfigurationOrDefault<string>("NoKey");
            Assert.That(value, Is.EqualTo(default(string)));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueDoesNotExist_IntDefaultReturned()
        {
            var value = _configHelper.GetConfigurationOrDefault<int>("NoKey");
            Assert.That(value, Is.EqualTo(default(int)));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueExistsAsBoolTrue_ValueReturned()
        {
            var actual = bool.Parse(GetAppSettingValue(KeyForBoolTrue));
            var value = _configHelper.GetConfigurationOrDefault<bool>(KeyForBoolTrue);
            Assert.That(value, Is.EqualTo(actual));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigValueDoesNotExistsAsBool_FalseReturned()
        {
            var value = _configHelper.GetConfigurationOrDefault<bool>("NoKey");
            Assert.That(value, Is.False);
        }

        #endregion

        #region ConnectionStrings

        [Test]
        public void GetConfigurationOrDefault_ConfigFromConnectionStrings_ValueReturned()
        {
            var expected = GetConnectionString(KeyForConn).ConnectionString;
            var value = _configHelper.GetConfigurationOrDefault<string>(KeyForConn);
            Assert.That(value, Is.EqualTo(expected), "ConnectionString was not found");
        }

        #endregion

        #region Invalid casts

        [Test]
        public void GetConfigurationValue_ConfigIsStringReturnedAsInt_FormatException()
        {
            var convertToType = typeof(int);
            var value = GetAppSettingValue(KeyForString);
            var exception = Assert.Throws<FormatException>(() => _configHelper.GetConfiguration<int>(KeyForString));
            Assert.That(exception.Message, Does.Contain(KeyForString));
            Assert.That(exception.Message, Does.Contain(value));
            Assert.That(exception.Message, Does.Contain(convertToType.FullName));
        }

        [Test]
        public void GetConfigurationOrDefault_ConfigIsStringReturnedAsInt_FormatException()
        {
            var convertToType = typeof(int);
            var value = GetAppSettingValue(KeyForString);
            var exception = Assert.Throws<FormatException>(() => _configHelper.GetConfigurationOrDefault<int>(KeyForString));
            Assert.That(exception.Message, Does.Contain(KeyForString));
            Assert.That(exception.Message, Does.Contain(value));
            Assert.That(exception.Message, Does.Contain(convertToType.FullName));
        }

        #endregion

        private static ConnectionStringSettings GetConnectionString(string key)
        {
            var val = ConfigurationManager.ConnectionStrings[key];
            if (val == null)
            {
                throw new AssertionException("The configuration needed for validation was not found. Review the test setup");
            }

            return val;
        }

        private static string GetAppSettingValue(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (val == null)
            {
                throw new AssertionException("The configuration needed for validation was not found. Review the test setup");
            }

            return val;
        }
    }
}
