using System.Net.WebSockets;

namespace TestBackend.Application.Services.Interfaces
{
    public interface IWebSocketService
    {
        Task AddSocketAsync(WebSocket webSocket);

        Task NotifyClientsAsync(string message);
    }
}
