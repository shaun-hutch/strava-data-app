using Newtonsoft.Json;
using Strava.Data.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Strava.Data.Core.Helpers
{
    public class HttpHelper
    {
        private static HttpClient Client;
        private static string ApiUrl;

        public static async Task Init(string apiUrl, string code = "")
        {
            ApiUrl = apiUrl;
            Client = new HttpClient() { BaseAddress = new Uri(ApiUrl) };

            await GetAuth(code);
        }

        public static async Task GetAuth(string code = "")
        {
            var service = new AuthService();
            var auth = await service.LoadAuth();

            if (auth == null)
            {
                Console.WriteLine("unable to acquire auth from JSON file.");

                if (!string.IsNullOrEmpty(code))
                {
                    auth = await service.GetAccessToken(code);
                    await service.WriteAuth(auth);
                }
                else
                {
                    Console.WriteLine("unable to get initial access token");
                    Environment.Exit(0);
                }
            }

            // refresh token if need be
            if (DateTime.UtcNow.Ticks > auth.ExpiryUtc.Ticks)
            {
                auth = await service.RefreshToken(auth.RefreshToken);
                if (!await service.WriteAuth(auth))
                {
                    Console.WriteLine("unable to write auth to JSON file.");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"Auth JSON written to {AppSettingsHelper.Settings.AuthJsonFile}");
                }
            }

            HttpHelper.SetAuth(auth.AccessToken);
        }

        public static void SetAuth(string accessToken) => 
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        public static async Task<HttpResult<T>> Get<T>(string route)
        {
            try
            {
                using var response = await Client.GetAsync(route);
                var result = await ProcessRequest<T>(response);

                return result;
            }
            catch (Exception ex)
            {
                return new HttpResult<T>(ex.ToString());
            }
        }

        public static async Task<HttpResult<T>> Post<T>(string route, object content = null)
        {
            HttpContent payload = null;
            if (content != null)
                payload = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            try
            {
                using var response = await Client.PostAsync(route, payload);
                var result = await ProcessRequest<T>(response);

                return result;
            }
            catch (Exception ex)
            {
                return new HttpResult<T>(ex.ToString());
            }
        }

        private static async Task<HttpResult<T>> ProcessRequest<T>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();

            ApiError error = null;
            T result = default;
            if (response.StatusCode != HttpStatusCode.OK)
                error = JsonConvert.DeserializeObject<ApiError>(data);
            else
                result = JsonConvert.DeserializeObject<T>(data);

            return new HttpResult<T>
            {
                Code = response.StatusCode,
                Result = result,
                Error = error
            };
        }
    }

    public class HttpResult<T>
    {
        public HttpResult() { }
        public HttpResult(string message)
        {
            this.Code = HttpStatusCode.InternalServerError;
            Result = default;
            Error = new ApiError
            {
                Message = message
            };
        }

        public T Result { get; set; }
        public HttpStatusCode Code { get; set; }
        public ApiError Error { get; set; }
    }

    public class ApiError
    {
        public string Message { get; set; }
        public Error[] Errors { get; set; }
    }

    public class Error
    {
        public string Resource { get; set; }
        public string Field { get; set; }
        public string Code { get; set; }
    }
}
