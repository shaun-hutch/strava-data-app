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
        public async Task<StravaAuth> SetAuthToken([FromBody] StravaAuthRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
                throw new ArgumentNullException(request.Code);

            var stravaAuth = await _service.GetAccessToken(request.Code);
            // this needs to put the whole auth (including refresh token) in a database for the user


            return stravaAuth;
        }

        [HttpPost("refresh")]
        public async Task<StravaAuth> RefreshToken([FromBody] string refreshToken)
        {
            var stravaAuth = await _service.RefreshToken(refreshToken);

            // put the auth in the database once more

            return stravaAuth;
        }

    }
}
