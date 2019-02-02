using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogEngine.Api.ViewModels;
using BlogEngine.Domain.Models;
using BlogEngine.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogEngine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }
        public UsersController(IUserService userService, IMapper mapper)
        {
            UserService = userService;
            Mapper = mapper;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public ActionResult<UserViewModel> Get(int id)
        {
            var foundUser = UserService.Find(id);
            if (foundUser == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<UserViewModel>(foundUser));
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<UserViewModel> Post(UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var persistedUser = UserService.AddUser(Mapper.Map<User>(viewModel));

            return Ok(Mapper.Map<UserViewModel>(persistedUser));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult<UserViewModel> Put(int id, UserInputViewModel viewModel)
        {
            var foundUser = UserService.Find(id);
            if (foundUser == null)
            {
                return BadRequest();
            }

            Mapper.Map(viewModel, foundUser);

            //foundUser.FirstName = viewModel.FirstName;
            //foundUser.LastName = viewModel.LastName;

            var persistedUser = UserService.UpdateUser(foundUser);

            return Ok(UserViewModel.ToViewModel(persistedUser));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var userDeleted = UserService.DeleteUser(id);

            return userDeleted ? (ActionResult)Ok() : (ActionResult)BadRequest();
        }
    }
}
