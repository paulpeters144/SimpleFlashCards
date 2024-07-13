using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards
{
    public class AppSettings
    {
        private IConfigurationRoot AppConfig { get; set; }
        public AppSettings()
        {
            AppConfig = getAppConfig();
        }
        public string GetSetting(string settingPath) =>
            AppConfig.GetSection(settingPath).Value;
        private IConfigurationRoot getAppConfig()
        {
            string appSettingsJsonPath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(appSettingsJsonPath)
            .AddJsonFile("appsettings.json", true, true);

            return builder.Build();
        }
    }
}
