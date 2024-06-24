using Microsoft.AspNetCore.SignalR;

namespace WebApplication1
{
    public class CRUDHub : Hub
    {
        public async Task SendMessage(string message, int postId)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, postId);
        }
    }
}
