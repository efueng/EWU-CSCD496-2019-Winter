using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Services.Interfaces;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    public class GroupUsersController : ControllerBase
    {
        private IGroupService GroupService { get; }

        public GroupUsersController(IGroupService groupService)
        {
            GroupService = groupService;
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> AddUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Log.Logger.Debug($"{nameof(groupId)} was invalid on call to {nameof(AddUserToGroup)}");
                Log.Logger.Error($"{nameof(groupId)} was invalid on call to {nameof(AddUserToGroup)}");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Log.Logger.Debug($"{nameof(userId)} was invalid on call to {nameof(AddUserToGroup)}");
                Log.Logger.Error($"{nameof(userId)} was invalid on call to {nameof(AddUserToGroup)}");
                return BadRequest();
            }

            if (await GroupService.AddUserToGroup(groupId, userId).ConfigureAwait(false))
            {
                Log.Logger.Information($"GroupUser was add to group on call to {nameof(AddUserToGroup)}");
                return Ok();
            }

            Log.Logger.Information($"GroupUser not added on call to {nameof(AddUserToGroup)}");
            return NotFound();
        }

        [HttpDelete("{groupId}")]
        public async Task<ActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Log.Logger.Debug($"{nameof(groupId)} was invalid on call to {nameof(RemoveUserFromGroup)}");
                Log.Logger.Error($"{nameof(groupId)} was invalid on call to {nameof(RemoveUserFromGroup)}");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Log.Logger.Debug($"{nameof(userId)} was invalid on call to {nameof(RemoveUserFromGroup)}");
                Log.Logger.Error($"{nameof(userId)} was invalid on call to {nameof(RemoveUserFromGroup)}");
                return BadRequest();
            }

            if (await GroupService.RemoveUserFromGroup(groupId, userId).ConfigureAwait(false))
            {
                Log.Logger.Information($"User was removed from group on call to {nameof(RemoveUserFromGroup)}");
                return Ok();
            }

            Log.Logger.Information($"User was not removed from group on call to {nameof(RemoveUserFromGroup)}");
            return NotFound();
        }
    }
}
