using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.Services.Interfaces;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class GroupUsersController : ControllerBase
    {
        private IGroupUserService GroupUserService { get; }
        private IMapper Mapper { get; }

        public GroupUsersController(IGroupUserService GroupUserService, IMapper mapper)
        {
            GroupUserService = GroupUserService;
            Mapper = mapper;
        }

        // GET api/GroupUser
        [HttpGet]
        [Produces(typeof(ICollection<GroupUserViewModel>))]
        public async Task<IActionResult> Get()
        {
            List<GroupUser> GroupUsers = await GroupUserService.FetchAll();
            return Ok(GroupUsers.Select(x => Mapper.Map<GroupUserViewModel>(x)));
            //return Ok(GroupUserService.FetchAll().Select(x => Mapper.Map<GroupUserViewModel>(x)));
        }

        [HttpGet("{id}")]
        [Produces(typeof(GroupUserViewModel))]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A GroupUser id must be specified");
            }
            GroupUser fetchedGroupUser = await GroupUserService.GetById(id);
            if (fetchedGroupUser == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<GroupUserViewModel>(fetchedGroupUser));
        }

        // POST api/GroupUser
        [HttpPost]
        [Produces(typeof(GroupUserViewModel))]
        public async Task<IActionResult> Post(GroupUserInputViewModel viewModel)
        {
            if (GroupUser == null)
            {
                return BadRequest();
            }

            GroupUser createdGroupUser = await GroupUserService.AddGroupUser(Mapper.Map<GroupUser>(viewModel));

            return CreatedAtAction(nameof(Get), new { id = createdGroupUser.Id }, Mapper.Map<GroupUserViewModel>(createdGroupUser));
        }

        // PUT api/GroupUser/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, GroupUserViewModel viewModel)
        {
            if (viewModel == null || id <= 0)
            {
                return BadRequest();
            }
            GroupUser fetchedGroupUser = await GroupUserService.GetById(id);
            if (fetchedGroupUser == null)
            {
                return NotFound();
            }

            Mapper.Map(viewModel, fetchedGroupUser);
            await GroupUserService.UpdateGroupUser(fetchedGroupUser);
            return NoContent();
        }

        // DELETE api/GroupUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A GroupUser id must be specified");
            }

            if (await GroupUserService.DeleteGroupUser(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
