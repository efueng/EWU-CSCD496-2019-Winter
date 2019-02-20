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
<<<<<<< refs/remotes/intellitect/Assignment6
        [Produces(typeof(ICollection<GroupViewModel>))]
        public async Task<IActionResult> Get()
        {
            List<Group> groups = await GroupService.FetchAll();
=======
        public async Task<ActionResult<ICollection<GroupViewModel>>> Get()
        {
            var groups = await GroupService.FetchAll();
>>>>>>> Initial start of code for assignment 7
            return Ok(groups.Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        [HttpGet("{id}")]
<<<<<<< refs/remotes/intellitect/Assignment6
        [Produces(typeof(GroupViewModel))]
        public async Task<IActionResult> Get(int id)
        {
            Group group = await GroupService.GetById(id);
=======
        public async Task<ActionResult<GroupViewModel>> Get(int id)
        {
            var group = await GroupService.GetById(id);
>>>>>>> Initial start of code for assignment 7
            if (group == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<GroupViewModel>(group));
        }

        // POST api/group
        [HttpPost]
<<<<<<< refs/remotes/intellitect/Assignment6
        [Produces(typeof(GroupViewModel))]
        public async Task<IActionResult> Post(GroupInputViewModel viewModel)
=======
        public async Task<ActionResult<GroupViewModel>> Post(GroupInputViewModel viewModel)
>>>>>>> Initial start of code for assignment 7
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
<<<<<<< refs/remotes/intellitect/Assignment6
            Group createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel));
=======
            var createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel));
>>>>>>> Initial start of code for assignment 7
            return CreatedAtAction(nameof(Get), new { id = createdGroup.Id}, Mapper.Map<GroupViewModel>(createdGroup));
        }

        // PUT api/group/5
        [HttpPut]
<<<<<<< refs/remotes/intellitect/Assignment6
        public async Task<IActionResult> Put(int id, GroupInputViewModel viewModel)
=======
        public async Task<ActionResult> Put(int id, GroupInputViewModel viewModel)
>>>>>>> Initial start of code for assignment 7
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
<<<<<<< refs/remotes/intellitect/Assignment6
            Group group = await GroupService.GetById(id);
=======
            var group = await GroupService.GetById(id);
>>>>>>> Initial start of code for assignment 7
            if (group == null)
            {
                return NotFound();
            }

            Mapper.Map(viewModel, group);
            await GroupService.UpdateGroup(group);

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
<<<<<<< refs/remotes/intellitect/Assignment6
        public async Task<IActionResult> Delete(int id)
=======
        public async Task<ActionResult> Delete(int id)
>>>>>>> Initial start of code for assignment 7
        {
            if (id <= 0)
            {
                return BadRequest("A group id must be specified");
            }

            if (await GroupService.DeleteGroup(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
