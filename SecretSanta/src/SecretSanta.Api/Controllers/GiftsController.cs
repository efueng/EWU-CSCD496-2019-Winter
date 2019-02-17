using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }

        public GiftsController(IGiftService giftService, IMapper mapper)
        {
            GiftService = giftService;
            Mapper = mapper;
        }

        // POST api/Gifts/5
        [HttpPost("{userId}")]
        [Produces(typeof(GiftViewModel))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddGiftForUser(int userId, GiftViewModel gift)
        {
            if (userId <= 0)
            {
                return NotFound();
            }

            if (gift == null)
            {
                return BadRequest();
            }

            return Created(nameof(AddGiftForUser), await GiftService.AddGiftToUser(userId, Mapper.Map<Gift>(gift)));
        }

        // GET api/Gifts/5
        [HttpGet("{userId}")]
        [Produces(typeof(ICollection<GiftViewModel>))]
        public async Task<IActionResult> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }

        // DELETE api/Gifts
        [HttpDelete]
        [Produces(typeof(bool))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGift(GiftViewModel viewModel)
        {
            if (viewModel == null)
            {
                //return Ok("Parameter viewModel cannot be null.");
                return BadRequest();
            }

            return Ok(await GiftService.DeleteGift(Mapper.Map<Gift>(viewModel)));
        }

        // PUT api/Gifts
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateGiftForUser(int userId, GiftViewModel viewModel)
        {
            if (viewModel == null || userId <= 0)
            {
                return BadRequest();
            }

            await GiftService.UpdateGiftForUser(userId, Mapper.Map<Gift>(viewModel));

            return NoContent();
        }
    }
}
