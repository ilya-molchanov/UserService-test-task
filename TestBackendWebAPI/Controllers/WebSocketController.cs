using Microsoft.AspNetCore.Mvc;
using TestBackend.Application.Services.Interfaces;

namespace TestBackend.WebApi.Controllers
{
    [Route("ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.AddSocketAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
