﻿using Newtonsoft.Json;
using Strava_App.Helpers;
using Strava_App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava_App.Services
{
    public class AuthService
    {
        private readonly int ClientId;
        private readonly string ClientSecret;
        private readonly string AuthJson;

        public AuthService()
        {
            ClientId = AppSettingsHelper.Settings.ClientId;
            ClientSecret = AppSettingsHelper.Settings.ClientSecret;
            AuthJson = AppSettingsHelper.Settings.AuthJsonFile;
        }
        public async Task<StravaAuth> GetAccessToken(string code)
        {
            var url = $"oauth/token";
            var authInfo = new
            {
                client_id = ClientId,
                client_secret = ClientSecret,
                code = code,
                grant_type = "authorization_code"
            };

            var auth = await HttpHelper.Post<StravaAuth>(url, authInfo);

            return auth.Result;
        }

        public async Task<StravaAuth> RefreshToken(string refreshToken)
        {
            var url = $"oauth/token";
            var authInfo = new
            {
                client_id = ClientId,
                client_secret = ClientSecret,
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };

            var auth = await HttpHelper.Post<StravaAuth>(url, authInfo);

            return auth.Result;
        }
        public async Task<StravaAuth> LoadAuth()
        {
            try
            {
                if (File.Exists(AuthJson))
                {
                    var authString = await File.ReadAllTextAsync(AuthJson);

                    return JsonConvert.DeserializeObject<StravaAuth>(authString);
                }

                else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return null;
            }
        }
        public async Task<bool> WriteAuth(StravaAuth auth)
        {
            try
            {
                await File.WriteAllTextAsync(AuthJson, JsonConvert.SerializeObject(auth));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return false;
            }
        }
    }
}