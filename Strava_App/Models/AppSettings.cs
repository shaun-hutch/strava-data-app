using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava_App.Models
{
    public class AppSettings
    {
        public int ClientId { get; set; }
        public string ClientSecret { get; set; } 
        public string ApiUrl { get; set; }
        public string AuthJsonFile { get; set; }
    }
}
