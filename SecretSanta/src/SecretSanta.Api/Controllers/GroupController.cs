using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get; }

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            GroupService = groupService;
            Mapper = mapper;
        }

        // GET api/group
        [HttpGet]
        //[ProducesResponseType(200)]
        //[Produces(typeof(IEnumerable<GroupViewModel>))]
        public IActionResult GetAllGroups()
        {
            return new OkObjectResult(GroupService
                .FetchAll()
                .Select(gs => Mapper.Map<GroupViewModel>(gs)));
        }

        // POST api/group
        [HttpPost]
        //[ProducesResponseType(200)] // ask why the assembly level directive doesn't work here, while it works above
        //[ProducesResponseType(400)]
        [Produces(typeof(GroupViewModel))]
        public IActionResult PostCreateGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            
            return CreatedAtAction(nameof(PostCreateGroup), Mapper.Map<GroupViewModel>(GroupService.AddGroup(Mapper.Map<Group>(viewModel))));
        }

        // PUT api/group/5
        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Produces(typeof(GroupViewModel))]
        public IActionResult PutUpdateGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            var fetchedGroup = GroupService.Find(id);
            if (fetchedGroup == null)
            {
                return NotFound();
            }

            fetchedGroup.Name = viewModel.Name;

            return Created(nameof(PutUpdateGroup), Mapper.Map<GroupViewModel>(GroupService.UpdateGroup(fetchedGroup)));
        }

        [HttpPut("{groupId}/{userid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutAddUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                return BadRequest();
            }

            if (userId <= 0)
            {
                return BadRequest();
            }

            if (GroupService.AddUserToGroup(groupId, userId))
            {
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        public IActionResult DeleteGroup(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A group id must be specified");
            }

            if (GroupService.DeleteGroup(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
