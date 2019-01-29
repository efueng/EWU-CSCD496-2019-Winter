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

        // PUT api/Group/someValue
        [HttpPut("{groupId}")]
        public ActionResult<DTO.Group> AddGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return new DTO.Group(_GroupService.AddGroup(group));
        }

        // PUT api/Group/someValue
        [HttpPut("{groupId}")]
        public ActionResult<DTO.Group> UpdateGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            return new DTO.Group(_GroupService.UpdateGroup(group));
        }

        // PUT api/Group/someValue
        //[HttpGet("{groupId}")]
        //public ActionResult<List<DTO.Group>> FetchAll()
        //{

        //}
    }
}
