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

        // GET api/Gift/5
        [HttpGet("{userId}")]
        public ActionResult<List<DTO.Gift>> GetGiftForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = _GiftService.GetGiftsForUser(userId);

            return databaseUsers.Select(x => new DTO.Gift(x)).ToList();
        }

        // PUT api/Gift/somevalue
        [HttpPut("{userId}")]
        public ActionResult<DTO.Gift> AddGiftToUser(int userId, DTO.Gift gift)
        {
            if (userId <= 0) return NotFound();

            Gift domainGift = new Gift
            {
                UserId = userId,
                Title = gift.Title,
                Description = gift.Description,
                Url = gift.Url,
                OrderOfImportance = gift.OrderOfImportance
            };

            _GiftService.AddGiftToUser(userId, domainGift);
            return gift;
            //return new DTO.Gift(_GiftService.AddGiftToUser(userId, domainGift));
        }

        // PATCH? api/Gift/somevalue
        [HttpPatch("{userId}")]
        public ActionResult<DTO.Gift> UpdateGiftForUser(int userId, Gift gift)
        {
            if (userId <= 0) return NotFound();

            return new DTO.Gift(_GiftService.UpdateGiftForUser(userId, gift));
        }

        // POST api/Gift/somevalue
        [HttpDelete("{userId}")]
        public void RemoveGiftForUser(int userId, Gift gift)
        {
            //if (userId <= 0)
            List<Gift> userGifts = _GiftService.GetGiftsForUser(userId);
            
            //foreach (Gift g in userGifts) _GiftService.RemoveGift(g);
            //userGifts.Select(g => _GiftService.RemoveGift(g));
            //List<DTO.Gift> dtoGifts = userGifts.Select(ug => new DTO.Gift(ug)).ToList();
            //dtoGifts.Select(dg => _GiftService.RemoveGift((Gift)dg));
            //_GiftService.RemoveGift(gift);
        }
    }
}
