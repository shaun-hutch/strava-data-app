using Strava.Data.Api.Helpers;
using Strava.Data.Shared;
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
        private const string KEY_ACTIVITIES = "activities";
        public async Task<Activity[]> Get()
        {
            Activity[] result;

            result = await CacheStore.Get<Activity[]>(KEY_ACTIVITIES);
            if (result == default(Activity[]))
            {
                var httpResult = await HttpHelper.Get<Activity[]>("activities");

                result = httpResult.Result;
                await CacheStore.Set(KEY_ACTIVITIES, result);
            }

            return result;
        }

        public async Task<Activity> GetById(long id)
        {
            Activity result;

            result = await CacheStore.Get<Activity>($"{id}");
            if (result == default(Activity))
            {
                var httpResult = await HttpHelper.Get<Activity>($"activities/{id}");

                result = httpResult.Result;
                await CacheStore.Set($"{id}", result);
            }

            return result;
        }
    }
}
