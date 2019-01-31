using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _GiftService;

        public GiftController(IGiftService giftService)
        {
            _GiftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
        }

        private Gift GiftDtoToEntity(DTO.Gift dto)
        {
            Gift entity = new Gift
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                OrderOfImportance = dto.OrderOfImportance,
                Url = dto.Url
            };

            return entity;
        }

        // GET api/Gift/5
        [HttpGet("{userId}")]
        public ActionResult<List<DTO.Gift>> GetGiftForUser(int userId)
        {
            if (userId <= 0)
            {
                //return NotFound("UserId must be greater than zero in GiftController.GetGiftForUser(int userId).");
                return NotFound();
            }
            List<Gift> databaseUsers = _GiftService.GetGiftsForUser(userId);

            return databaseUsers.Select(x => new DTO.Gift(x)).ToList();
        }

        // POST api/Gift/somevalue
        [HttpPost("{userId}")]
        public ActionResult AddGiftToUser(int userId, DTO.Gift gift)
        {
            if (userId <= 0) return NotFound();

            if (gift == null) return BadRequest();

            var entity = GiftDtoToEntity(gift);
            _GiftService.AddGiftToUser(userId, entity);

            return Ok();
        }

        // PATCH? api/Gift/somevalue
        [HttpPut("{userId}")]
        public ActionResult UpdateGiftForUser(int userId, DTO.Gift gift)
        {
            if (userId <= 0) return NotFound();

            var entity = GiftDtoToEntity(gift);
            _GiftService.UpdateGiftForUser(userId, entity);

            return Ok();
        }

        // POST api/Gift/somevalue
        [HttpDelete("{userId}")]
        public ActionResult RemoveGiftForUser( DTO.Gift gift)
        {
            //if (userId <= 0) return NotFound();
            if (gift == null) return BadRequest();

            var entity = GiftDtoToEntity(gift);
            //entity.UserId = userId;
            _GiftService.RemoveGift(entity);

            return Ok();
        }
    }
}
