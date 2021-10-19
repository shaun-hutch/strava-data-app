using Strava.Data.Api.Helpers;
using Strava.Data.Shared.Models;
using System.Threading.Tasks;

namespace Strava.Data.Api.Services
{
    public interface IActivityService
    {
        public Task<Activity[]> Get();

        public Task<Activity> GetById(long id);

    }
    public class ActivityService : IActivityService
    {
        public async Task<Activity[]> Get()
        {
            var result = await HttpHelper.Get<Activity[]>("activities");

            return result.Result;
        }

        public async Task<Activity> GetById(long id)
        {
            var result  = await HttpHelper.Get<Activity>($"activities/{id}");

            return result.Result;
        }
    }
}
