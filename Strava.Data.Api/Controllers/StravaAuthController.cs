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
        private IAuthService _service;

        public StravaAuthController(IAuthService service)
        {
            _service = service;
        }


        [HttpGet("token")]
        public async Task<StravaAuth> GetNewToken(string code)
        {
            var stravaAuth = await _service.GetAccessToken(code);

            return stravaAuth;
        }

        [HttpGet("refresh")]
        public async Task<StravaAuth> RefreshToken(string refreshToken)
        {
            var stravaAuth = await _service.RefreshToken(refreshToken);

            return stravaAuth;
        }

    }
}
