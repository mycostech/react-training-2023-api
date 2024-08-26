using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReactTraining2023.Hubs
{
    public class ScoreHub : Hub
    {
        public static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _appScores = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

        public async Task CreateApp(string appName)
        {
             _appScores.GetOrAdd(appName, new ConcurrentDictionary<string, int>());
            await Groups.AddToGroupAsync(Context.ConnectionId, appName);
        }

        public async Task LeaveApp(string appName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appName);
        }
    }
}