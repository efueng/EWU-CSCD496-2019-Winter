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
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; set; }

        public UserController(IUserService userService, IMapper mapper)
        {
            UserService = userService;
            Mapper = mapper;
        }

        // POST api/<controller>
        [HttpPost]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(200)]
        [Produces(typeof(UserViewModel))]
        public IActionResult PostAddUser(UserInputViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest();
            }

            var persistedUser = UserService.AddUser(Mapper.Map<User>(userViewModel));
            //var persistedUser = UserService.AddUser(UserInputViewModel.ToModel(userViewModel));

            return Created("", Mapper.Map<UserViewModel>(persistedUser));
            //return Ok(Mapper.Map<UserViewModel>(persistedUser));
            //return Ok(UserViewModel.ToViewModel(persistedUser));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(200)]
        [Produces(typeof(UserViewModel))]
        public IActionResult PutUpdateUser(int id, UserInputViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest();
            }

            var foundUser = UserService.Find(id);
            if (foundUser == null)
            {
                return NotFound();
            }

            foundUser.FirstName = userViewModel.FirstName;
            foundUser.LastName = userViewModel.LastName;

            var persistedUser = UserService.UpdateUser(foundUser);
            //var persistedUser = UserService.UpdateUser(foundUser);

            return Ok(Mapper.Map<UserViewModel>(persistedUser));
            //return Ok(UserViewModel.ToViewModel(persistedUser));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Produces(typeof(ActionResult))]
        public IActionResult DeleteUser(int id)
        {
            bool userWasDeleted = UserService.DeleteUser(id);

            return userWasDeleted ? (ActionResult)Ok() : (ActionResult)NotFound();
        }
    }
}
