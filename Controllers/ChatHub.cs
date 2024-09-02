using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message, int senderId, int receiverId)
    {
        // Validate parameters
        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("User and message cannot be null or empty");
        }

        // Add your server-side logic here
        // For example, save the message to a database

        // Broadcast the message to all connected clients
        await Clients.All.SendAsync("ReceiveMessage", user, message, senderId, receiverId);
    }
}