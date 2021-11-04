using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Strava.Data.Api.Controllers;
using Strava.Data.Api.Helpers;
using Strava.Data.Api.Services;
using Strava.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Strava.Data.Shared;

namespace Strava.Data.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IUserService, UserService>();

            services.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            var section = Configuration.GetSection("AppSettings");
            var clientId = section.GetValue<int>("clientId");
            var clientSecret = section.GetValue<string>("clientSecret");

            var code = "";
            await HttpHelper.Init("https://www.strava.com/api/v3/", code, clientId, clientSecret);
            services.AddMemoryCache();
            // startup a cache store

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthorization();

            CacheStore.Cache = cache;

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
        }
    }
}
