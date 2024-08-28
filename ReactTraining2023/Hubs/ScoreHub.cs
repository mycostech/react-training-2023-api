using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ReactTraining2023.Hubs
{
    public class ScoreHub : Hub
    {
        public static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _appScores = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
        private static int _connectionCount = 0;

        // Method called when a client connects
        public override Task OnConnectedAsync()
        {
            // Increment the connection count
            _connectionCount++;

            // Notify all clients about the updated count
            Clients.All.SendAsync("UpdateConnectionCount", _connectionCount);

            return base.OnConnectedAsync();
        }

        // Method called when a client disconnects
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Decrement the connection count
            _connectionCount--;

            // Notify all clients about the updated count
            Clients.All.SendAsync("UpdateConnectionCount", _connectionCount);

            return base.OnDisconnectedAsync(exception);
        }
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