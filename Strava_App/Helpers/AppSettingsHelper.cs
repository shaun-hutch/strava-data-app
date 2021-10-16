﻿using Newtonsoft.Json;
using Strava_App.Models;
using System.IO;

namespace Strava_App.Helpers
{
    public class AppSettingsHelper
    {
        public static AppSettings Settings;

        public static void LoadAppSettings()
        {
            var settingsFile = File.ReadAllText("appsettings.json");
            Settings = JsonConvert.DeserializeObject<AppSettings>(settingsFile);

#if DEBUG
            var debugFile = "appsettings.debug.json";
            if (File.Exists(debugFile))
            {
                var debugSettingsFile = File.ReadAllText(debugFile);
                var debugSettings = JsonConvert.DeserializeObject<AppSettings>(debugSettingsFile);

                if (!string.IsNullOrEmpty(debugSettings.ApiUrl))
                    Settings.ApiUrl = debugSettings.ApiUrl;
                if (!string.IsNullOrEmpty(debugSettings.AuthJsonFile))
                    Settings.AuthJsonFile = debugSettings.AuthJsonFile;
                if (debugSettings.ClientId > 0)
                    Settings.ClientId = debugSettings.ClientId;
                if (!string.IsNullOrEmpty(debugSettings.ClientSecret))
                    Settings.ClientSecret = debugSettings.ClientSecret;
            }
#endif
        }
    }
}