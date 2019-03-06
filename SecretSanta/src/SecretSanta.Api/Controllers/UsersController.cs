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
    public class UsersController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }

        public UsersController(IUserService userService, IMapper mapper)
        {
            UserService = userService;
            Mapper = mapper;
        }

        // GET api/User
        [HttpGet]
        public async Task<ActionResult<ICollection<UserViewModel>>> GetAllUsers()
        {
            List<User> users = await UserService.FetchAll().ConfigureAwait(false);

            Log.Logger.Information($"Returning ICollection<UserViewModel> from {nameof(GetAllUsers)}");
            return Ok(users.Select(x => Mapper.Map<UserViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(int id)
        {
            User fetchedUser = await UserService.GetById(id).ConfigureAwait(false);
            if (fetchedUser == null)
            {
                Log.Logger.Error($"{nameof(id)} produced a null user on call to {nameof(GetUserById)}");
                Log.Logger.Debug($"{nameof(id)} produced a null user on call to {nameof(GetUserById)}");
                return NotFound();
            }

            Log.Logger.Information($"Returning UserViewModel from {nameof(GetUserById)}", id);
            return Ok(Mapper.Map<UserViewModel>(fetchedUser));
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Log.Logger.Error($"{nameof(viewModel)} produced a null user on call to {nameof(CreateUser)}");
                Log.Logger.Debug($"{nameof(viewModel)} produced a null user on call to {nameof(CreateUser)}");
                return BadRequest();
            }

            User createdUser = await UserService.AddUser(Mapper.Map<User>(viewModel)).ConfigureAwait(false);

            Log.Logger.Information($"User was created and added on call to {nameof(CreateUser)}");
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, Mapper.Map<UserViewModel>(createdUser));
        }

        // PUT api/User/5
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Log.Logger.Error($"{nameof(viewModel)} was null on call to {nameof(GetUserById)}");
                Log.Logger.Debug($"{nameof(viewModel)} was null on call to {nameof(GetUserById)}");
                return BadRequest();
            }
            User fetchedUser = await UserService.GetById(id).ConfigureAwait(false);
            if (fetchedUser == null)
            {
                Log.Logger.Error($"{nameof(fetchedUser)} was null on call to {nameof(GetUserById)}");
                Log.Logger.Debug($"{nameof(fetchedUser)} was null on call to {nameof(GetUserById)}");
                return NotFound();
            }

            Mapper.Map(viewModel, fetchedUser);
            await UserService.UpdateUser(fetchedUser).ConfigureAwait(false);

            Log.Logger.Information($"User was updated on call to {nameof(UpdateUser)}");
            return NoContent();
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                Log.Logger.Error($"{nameof(id)} was invalid on call to {nameof(DeleteUser)}");
                Log.Logger.Debug($"{nameof(id)} was invalid on call to {nameof(DeleteUser)}");
                return BadRequest("A User id must be specified");
            }

            if (await UserService.DeleteUser(id).ConfigureAwait(false))
            {
                Log.Logger.Information($"User was deleted on call to {nameof(DeleteUser)}", id);
                return Ok();
            }

            Log.Logger.Information($"User was not deleted on call to {nameof(DeleteUser)}", id);
            return NotFound();
        }
    }
}
