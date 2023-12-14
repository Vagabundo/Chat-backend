using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicChat.Hubs;
using PublicChat.Models;
using PublicChat.Services;

namespace PublicChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IAIChatService _AIChatService;

        public ChatController(IHubContext<ChatHub> hubContext, IAIChatService AIChatService)
        {
            _hubContext = hubContext;
            _AIChatService = AIChatService;
        }

        [Route("send")]                                           //path looks like this: https://localhost:5001/api/chat/send
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] Message msg)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveOne", msg.user, msg.msgText);

            if (msg.msgText.StartsWith("AI:"))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveOne", "AI", await _AIChatService.GetAnswer(msg.msgText.Split(':').Last()));
            }
            return Ok();
        }
    }
}
