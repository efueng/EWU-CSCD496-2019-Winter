using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _GroupService;

        public GroupController(IGroupService groupService)
        {
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        private Group GroupDtoToEntity(DTO.Group dto)
        {
            Group entity = new Group
            {
                Id = dto.Id,
                Name = dto.Name,
                GroupUsers = new List<GroupUser>()
            };

            return entity;
        }

        // POST api/Group
        [HttpPost]
        public ActionResult AddGroup(DTO.Group dtoGroup)
        {
            if (dtoGroup == null)
            {
                return BadRequest("dtoGroup parameter was null on call to GroupController.AddGroup(DTO.Group dtoGroup).");
            }

            _GroupService.AddGroup(GroupDtoToEntity(dtoGroup));

            return Ok();
        }

        // PUT api/Group
        [HttpPut]
        public ActionResult UpdateGroup(DTO.Group dtoGroup)
        {
            if (dtoGroup == null)
            {
                return BadRequest("dtoGroup parameter was null on call to GroupController.UpdateGroup(DTO.Group dtoGroup).");
            }
            
            _GroupService.AddGroup(GroupDtoToEntity(dtoGroup));

            return Ok();
        }

        // DELETE api/Group/
        [HttpDelete]
        public ActionResult RemoveGroup(DTO.Group dtoGroup)
        {
            if (dtoGroup == null)
            {
                return BadRequest("dtoGroup parameter was null on call to GroupController.RemoveGroup(DTO.Group dtoGroup).");
            }

            _GroupService.RemoveGroup(GroupDtoToEntity(dtoGroup));

            return Ok();
        }

        // GET api/Group
        [HttpGet]
        public ActionResult<List<DTO.Group>> FetchAll()
        {
            return _GroupService.FetchAll().Select(x => new DTO.Group(x)).ToList();
        }
    }
}
