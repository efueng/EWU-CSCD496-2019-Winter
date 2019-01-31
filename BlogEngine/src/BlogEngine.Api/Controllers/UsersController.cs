using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Api.ViewModels;
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
        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<UserViewModel> Get(int id)
        {
            var foundUser = UserService.Find(id);
            if (foundUser == null)
            {
                return NotFound();
            }

            return Ok(UserViewModel.ToViewModel(foundUser));
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<UserViewModel> Post(UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var persistedUser = UserService.AddUser(UserInputViewModel.ToModel(viewModel));

            return Ok(UserViewModel.ToViewModel(persistedUser));
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

            foundUser.FirstName = viewModel.FirstName;
            foundUser.LastName = viewModel.LastName;

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
