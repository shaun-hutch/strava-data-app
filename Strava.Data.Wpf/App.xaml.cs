using Strava.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Strava.Data.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Trace.WriteLine("startup");

            AppSettingsHelper.LoadAppSettings();
            Task t = HttpHelper.Init(AppSettingsHelper.Settings.ApiUrl);
            t.Wait();
        }

    }
}
