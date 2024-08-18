using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReactTraining2023.Hubs
{
    public class ScoreHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _appScores = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

        public async Task JoinApp(string appName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, appName);
        }

        public async Task LeaveApp(string appName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appName);
        }

        public async Task RemoveTeam(string appName, string team)
        {
            if (_appScores.TryGetValue(appName, out var scores))
            {
                if (scores.TryRemove(team, out _))
                {
                    await Clients.Group(appName).SendAsync("ReceiveScores", scores);
                }
            }
        }

        public async Task GetScores(string appName)
        {
            if (_appScores.TryGetValue(appName, out var scores))
            {
                // Send the current scores to the caller
                await Clients.Caller.SendAsync("ReceiveScores", scores);
            }
        }

        public async Task UpdateScore(string appName, string team, int score)
        {
            // Update the score for the given team
            var scores = _appScores.GetOrAdd(appName, new ConcurrentDictionary<string, int>());

            scores[team] = score;

            // Broadcast the updated scores to all connected clients
            await Clients.Group(appName).SendAsync("ReceiveScores", scores);
        }
    }
}