using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get; }

        public GroupsController(IGroupService groupService, IMapper mapper)
        {
            GroupService = groupService;
            Mapper = mapper;
        }

        // GET api/group
        [HttpGet]
        public async Task<ActionResult<ICollection<GroupViewModel>>> GetGroups()
        {
            List<Group> groups = await GroupService.FetchAll().ConfigureAwait(false);
            Log.Logger.Information($"Groups fetched on call to {nameof(GetGroups)}");
            return Ok(groups.Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupViewModel>> GetGroup(int id)
        {
            Group group = await GroupService.GetById(id).ConfigureAwait(false);
            if (group == null)
            {
                Log.Logger.Debug($"{nameof(id)} produced a null group on call to {nameof(GetGroup)}");
                Log.Logger.Error($"{nameof(id)} produced a null group on call to {nameof(GetGroup)}");

                return NotFound();
            }

            return Ok(Mapper.Map<GroupViewModel>(group));
        }

        // POST api/group
        [HttpPost]
        public async Task<ActionResult<GroupViewModel>> CreateGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Log.Logger.Debug($"{nameof(viewModel)} was null on call to {nameof(GetGroup)}");
                Log.Logger.Error($"{nameof(viewModel)} was null on call to {nameof(GetGroup)}");
                return BadRequest();
            }

            Group createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel)).ConfigureAwait(false);

            Log.Logger.Information($"Group was created and added on call to {nameof(CreateGroup)}", viewModel);
            return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id}, Mapper.Map<GroupViewModel>(createdGroup));
        }

        // PUT api/group/5
        [HttpPut]
        public async Task<ActionResult> UpdateGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Log.Logger.Debug($"{nameof(viewModel)} was null on call to {nameof(GetGroup)}", id);
                Log.Logger.Error($"{nameof(viewModel)} was null on call to {nameof(GetGroup)}", id);
                return BadRequest();
            }
            Group group = await GroupService.GetById(id).ConfigureAwait(false);
            if (group == null)
            {
                Log.Logger.Debug($"{nameof(id)} produced a null group on call to {nameof(GetGroup)}", viewModel);
                Log.Logger.Error($"{nameof(id)} produced a null group on call to {nameof(GetGroup)}", viewModel);
                return NotFound();
            }

            Mapper.Map(viewModel, group);
            await GroupService.UpdateGroup(group).ConfigureAwait(false);

            Log.Logger.Information($"Group was updated on call to {nameof(UpdateGroup)}");
            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            if (id <= 0)
            {
                Log.Logger.Debug($"{nameof(id)} was invalid on call to {nameof(GetGroup)}");
                Log.Logger.Error($"{nameof(id)} was invalid on call to {nameof(GetGroup)}");
                return BadRequest("A group id must be specified");
            }

            if (await GroupService.DeleteGroup(id).ConfigureAwait(false))
            {
                Log.Logger.Information($"Group was deleted on call to {nameof(DeleteGroup)}", id);
                return Ok();
            }

            Log.Logger.Information($"No group found on call to {nameof(DeleteGroup)}", id);
            return NotFound();
        }
    }
}
