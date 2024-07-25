using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command) {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityID.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityID = httpContext.Request.Query["activityID"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityID);
            
            var result = await _mediator.Send(new List.Query{ActivityID = Guid.Parse(activityID)});
            
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}