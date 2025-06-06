﻿using System.Net.WebSockets;
using System.Text;
using TestBackend.Application.Services.Interfaces;

namespace TestBackend.Application.Services
{
    public class WebSocketService : IWebSocketService
    {
        private static readonly List<WebSocket> _connections = new();

        public async Task AddSocketAsync(WebSocket webSocket)
        {
            _connections.Add(webSocket);
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connections.Remove(webSocket);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
            }
        }

        public async Task NotifyClientsAsync(string message)
        {
            var tasks = _connections
            .Where(s => s != null && s.State == WebSocketState.Open)
            .Select(s => s.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                WebSocketMessageType.Text, true, CancellationToken.None));

            await Task.WhenAll(tasks);
        }
    }
}
