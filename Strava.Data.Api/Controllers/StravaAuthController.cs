using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Strava.Data.Api.Services;
using Strava.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strava.Data.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class StravaAuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public StravaAuthController(IAuthService service)
        {
            _service = service;
        }


        [HttpPost("token")]
        public async Task<StravaAuth> GetNewToken([FromBody] string code)
        {
            var stravaAuth = await _service.GetAccessToken(code);

            Globals.AccessToken = stravaAuth.AccessToken;
            Globals.RefreshToken = stravaAuth.RefreshToken;
            Globals.ExpiresAt = stravaAuth.ExpiresAt;

            return stravaAuth;
        }

        [HttpPost("refresh")]
        public async Task<StravaAuth> RefreshToken([FromBody] string refreshToken)
        {
            var stravaAuth = await _service.RefreshToken(refreshToken);

            return stravaAuth;
        }

    }
}
