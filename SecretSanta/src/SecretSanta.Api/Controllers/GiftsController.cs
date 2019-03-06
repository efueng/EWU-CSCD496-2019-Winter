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
    public class GiftsController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }

        public GiftsController(IGiftService giftService, IMapper mapper)
        {
            GiftService = giftService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
            Gift gift = await GiftService.GetGift(id).ConfigureAwait(false);

            if (gift == null)
            {
                Log.Logger.Debug($"{nameof(id)} was invalid on call to {nameof(GetGift)}");
                Log.Logger.Error($"{nameof(id)} was invalid on call to {nameof(GetGift)}");
                return NotFound();
            }

            Log.Logger.Information($"Returning GiftViewModel from {nameof(GetGift)}", id);
            return Ok(Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
            Gift createdGift = await GiftService.AddGift(Mapper.Map<Gift>(viewModel)).ConfigureAwait(false);

            Log.Logger.Information($"Gift was created and added on call to {nameof(CreateGift)}", viewModel);
            return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                Log.Logger.Debug($"{nameof(userId)} was invalid on call to {nameof(GetGiftsForUser)}");
                Log.Logger.Error($"{nameof(userId)} was invalid on call to {nameof(GetGiftsForUser)}");
                return NotFound();
            }
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId).ConfigureAwait(false);
            Log.Logger.Information($"Returning ICollection<GiftViewModel> from {nameof(GetGiftsForUser)}", userId);
            return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
