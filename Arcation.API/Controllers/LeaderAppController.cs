using Arcation.API.Extentions;
using Arcation.Core;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin, User")]
    public class LeaderAppController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LeaderAppController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/Leaderapp
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            Leader queryLeader = await _unitOfWork.Leaders.GetLeaderDetail(HttpContext.GetUserId(), HttpContext.GetBusinessId());

            if(queryLeader != null)
            {
                var locations = await _unitOfWork.Locations.LocaionsWithBandsRelatedToLeader(HttpContext.GetUserId(), HttpContext.GetBusinessId());
                var list = new List<LeaderLocations>();
                var locationIds = queryLeader.BandLocationLeaders.Select(e => e.BandLocation.LocationId);

                foreach (int LocationId in locationIds)
                {
                    Location queryLocation = await _unitOfWork.Locations.FindAsync(e => e.Id == LocationId);
                    var bandIds = queryLeader.BandLocationLeaders.Where(e => e.BandLocation.LocationId == LocationId).Select(e => e.BandLocation.BandId);
                    List<LeaderLocationBands> leaderLocationBands = new();

                    foreach (int bandId in bandIds)
                    {

                        Band queryBand = await _unitOfWork.Bands.FindAsync(e => e.Id == bandId);
                        BandLocationLeader queryBandLocationLeader = queryLeader.BandLocationLeaders.FirstOrDefault(e => e.BandLocation.LocationId == LocationId && e.BandLocation.BandId == bandId);
                        LeaderLocationBands leaderLocationBands1 = new LeaderLocationBands
                        {
                            BandId = bandId,
                            BandName = queryBand.BandName,
                            BandLocationLeaderId = queryBandLocationLeader.Id
                        };
                        leaderLocationBands.Add(leaderLocationBands1);
                    }

                    LeaderLocations result = new LeaderLocations
                    {
                        LocationName = queryLocation.LocationName,
                        LocationId = LocationId,
                        BandLocations = leaderLocationBands

                    };
                    list.Add(result);
                }
                return Ok(list);
            }
            return NotFound();
        }

        // api/Leaderapp/periods/{bandLocationLeaderId}
        [HttpGet("periods/{bandLocationLeaderId}")]
        public async Task<IActionResult> GetPeriods([FromRoute] int? bandLocationLeaderId)
        {
            if(bandLocationLeaderId != null)
            {
                var periods = await _unitOfWork.BandLocationLeaderPeriods.GetPeriods(bandLocationLeaderId, HttpContext.GetUserId(), HttpContext.GetBusinessId());
                if (periods != null)
                {
                    return Ok(_mapper.Map<IEnumerable<PeriodsLeaderApp>>(periods));
                }
                return NotFound();
            }
            return NotFound();
        }

    }
}
