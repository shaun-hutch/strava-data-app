using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Strava.Data.Api.Helpers;
using Strava.Data.Shared.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Strava.Data.Api.Services
{
    public interface IAuthService
    {
        public Task<StravaAuth> GetAccessToken(string code);
        public Task<StravaAuth> RefreshToken(string refreshToken);
    }

    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly string authJson = "auth.json";

        private int _clientId;
        private string _clientSecret;
        public AuthService() { }

        public AuthService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _clientId = _appSettings.ClientId;
            _clientSecret = _appSettings.Secret;
        }

        public void SetInfo(int clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<StravaAuth> GetAccessToken(string code)
        {
            var url = $"oauth/token";
            var authInfo = new
            {
                client_id = _appSettings.ClientId > 0 ? _appSettings.ClientId : _clientId,
                client_secret = _appSettings.ClientSecret ?? _clientSecret,
                code,
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
                client_id = _appSettings?.ClientId > 0 ? _appSettings?.ClientId : _clientId,
                client_secret = _appSettings?.ClientSecret ?? _clientSecret,
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
                if (File.Exists(_appSettings?.AuthJsonFile ?? authJson))
                {
                    var authString = await File.ReadAllTextAsync(_appSettings?.AuthJsonFile ?? authJson);

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
                await File.WriteAllTextAsync(_appSettings?.AuthJsonFile ?? authJson, JsonConvert.SerializeObject(auth));

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
