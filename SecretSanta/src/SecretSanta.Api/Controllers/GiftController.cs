using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// [assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }

        public GiftController(IGiftService giftService, IMapper mapper)
        {
            GiftService = giftService;
            Mapper = Mapper;
        }

        // GET api/Gift/5
        [HttpGet("{userId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces(typeof(List<GiftViewModel>))]
        public IActionResult GetGiftForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers
                .Select(du => Mapper.Map<GiftViewModel>(du))
                .ToList());
        }
    }
}
