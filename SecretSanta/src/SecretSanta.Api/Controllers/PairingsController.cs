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
        [HttpPost("{groupId}")]
        [Produces(typeof(List<PairingViewModel>))]
        public async Task<IActionResult> Post(int groupId)
        {
            if (groupId <= 0)
            {
                return BadRequest();
            }

            //var pairings = (await PairingService.GeneratePairings(groupId)).Select(x => Mapper.Map<PairingViewModel>(x)).ToList();
            List<Pairing> pairings = await PairingService.GeneratePairings(groupId);
            List<PairingViewModel> viewModels = pairings.Select(x => Mapper.Map<PairingViewModel>(x)).ToList();
            if (viewModels != null)
            {
                return Created(nameof(Post), viewModels);
            }

            return NotFound();
        }
    }
}
