using Microsoft.AspNetCore.Mvc;
using Strava.Data.Api.Services;
using Strava.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strava.Data.Api.Controllers
{
    [ApiController]
    [Route("activities")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;
        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<Activity[]> GetAll()
        {
            var activities = await _service.Get();

            return activities;
        }

        [HttpGet("{id}")]
        public async Task<Activity> GetById(long id)
        {
            var activity = await _service.GetById(id);

            return activity;
        }
    }
}
