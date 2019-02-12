using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingsController : ControllerBase
    {
        private IPairingService PairingService { get; }
        private IMapper Mapper { get; }

        public PairingsController(IPairingService pairingService, IMapper mapper)
        {
            PairingService = pairingService;
            Mapper = mapper;
        }

        // GET: api/Pairing
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Get(int groupId)
        {
            if (groupId <= 0)
            {
                return BadRequest();
            }

            if (await PairingService.GeneratePairings(groupId))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
