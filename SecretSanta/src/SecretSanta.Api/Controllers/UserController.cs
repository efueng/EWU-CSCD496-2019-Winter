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
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        
        public UserController(IUserService userService)
        {
            _UserService = userService;
        }

        private User UserDtoToEntity(DTO.User dto)
        {
            User entity = new User
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gifts = new List<Gift>(),
                GroupUsers = new List<GroupUser>()
            };

            return entity;
        }

        // PUT api/User
        [HttpPut]
        public ActionResult AddUser(DTO.User dtoUser)
        {
            if (dtoUser == null) return BadRequest();

            _UserService.AddUser(UserDtoToEntity(dtoUser));

            return Ok();
        }

        // PUT api/User
        [HttpPut]
        public ActionResult UpdateUser(DTO.User dtoUser)
        {
            if (dtoUser == null) return BadRequest();

            _UserService.UpdateUser(UserDtoToEntity(dtoUser));

            return Ok();
        }

        // GET api/User
        public ActionResult<List<DTO.User>> FetchAll()
        {
            return _UserService.FetchAll().Select(x => new DTO.User(x)).ToList();
        }
    }
}
