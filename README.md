# NetConfigurationHelper
## Purpose
Simplify unit testing and mocking of configurations.

## Installation
Currently there is no package uploaded to NuGet gallery.
The workaround is to either build your own package using the nuspec in the repo.
A less preferred method is to include the code directly.

## Usage
```C#
import Wiksoft.NetConfigurationHelper

IConfigurationHelper _config = new ConfigurationHelper();
var configValue = _config.GetConfiguration<Type>("ConfigKey");
var otherValue = _config.GetConfigurationOrDefault<Type>("Key", "DefaultIfKeyDoesNotExist");
```
