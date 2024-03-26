namespace API.SignalR;
public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = 
        new Dictionary<string, List<string>>();

    public Task UserConnected(string username, string conntectionId)
    {
        lock(OnlineUsers)
        {
            if(OnlineUsers.ContainsKey(username)) {
                OnlineUsers[username].Add(conntectionId);
            }
            else {
                OnlineUsers.Add(username, new List<string>{conntectionId});
            }
        }

        return Task.CompletedTask;
    }

    public Task UserDisconnected(string username, string conntectionId)
    {
        lock(OnlineUsers)
        {
            if(!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;

            OnlineUsers[username].Remove(conntectionId);

            if(OnlineUsers[username].Count == 0) {
                OnlineUsers.Remove(username);
            }
        }

        return Task.CompletedTask;
    }

    public Task<string[]> GetOnlineUsers() 
    {
        string[] onlineUsers;
        lock(OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(key => key.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }
}
