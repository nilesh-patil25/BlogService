using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.Core.AppSettings
{
    public class AppSettingsHelper
    {
        private static IConfiguration _configuration;
        public static void AppSettingConfigure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetValue(string key) => _configuration.GetSection(key).Value;
    }
}
