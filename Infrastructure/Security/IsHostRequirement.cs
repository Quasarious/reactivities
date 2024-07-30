using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
        
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement> 
    {
        private readonly DataContext _ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(DataContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userID == null)
                return Task.CompletedTask;
            
            var activityID = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .FirstOrDefault(x => x.Key == "id").Value?.ToString());
                        
            var attendee = _ctx.ActivityAttendees
                .FirstOrDefaultAsync(aa => aa.AppUserID == userID && aa.ActivityID == activityID)
                .Result;

            if (attendee == null)
                return Task.CompletedTask;

            if (attendee.IsHost) {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}