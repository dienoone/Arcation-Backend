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
    [Authorize(Roles ="Admin")]
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // TODO: ADD AUTHORIZE ATTRIBUTE:
        public SearchController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/Search/Company/{name}
        [HttpGet("Company/{name}")]
        public async Task<IActionResult> SearchCompany([FromRoute] string? name)
        {
            if(name != null)
            {
                IEnumerable<Company> entities = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.Name.Contains(name));
                return Ok(_mapper.Map<IEnumerable<CompanyViewModel>>(entities));
            }
            else
            {
                IEnumerable<Company> entities = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId());
                return Ok(_mapper.Map<IEnumerable<CompanyViewModel>>(entities));
            }
        }

        // api/Search/Company/{companyId}/Location/{name}
        [HttpGet("Company/{companyId}/Location/{name}")]
        public async Task<IActionResult> SearchLocation([FromRoute] int? companyId, [FromRoute] string? name)
        {
            if (companyId != null)
            {
                if(name != null)
                {
                    IEnumerable<Location> entities = await _unitOfWork.Locations.SearchLocationAsync(HttpContext.GetBusinessId(), companyId, name);
                    return Ok(_mapper.Map<IEnumerable<LocationViewModel>>(entities));
                }
                else
                {
                    IEnumerable<Location> entities = await _unitOfWork.Locations.GetAllLocationsAsync(HttpContext.GetBusinessId(), companyId);
                    return Ok(_mapper.Map<IEnumerable<LocationViewModel>>(entities));
                }
            }
            return NotFound();
        }

        // api/Search/Bills/{bandLocationId}/Bill/{name}
        [HttpGet("Bills/{bandLocationId}/Bill/{name}")]
        public async Task<IActionResult> SearchBill([FromRoute] int? bandLocationId, [FromRoute] string? name)
        {
            if (bandLocationId != null)
            {
                if(name != null)
                {
                    IEnumerable<Bill> entities = await _unitOfWork.Bills.FindAllAsync(b => b.BusinessId == HttpContext.GetBusinessId() && !b.IsDeleted && b.BandLocationId == bandLocationId && b.BillCode == name);
                    return Ok(_mapper.Map<IEnumerable<BillDto>>(entities));
                }
                else
                {
                    IEnumerable<Bill> entities = await _unitOfWork.Bills.FindAllAsync(b => b.BusinessId == HttpContext.GetBusinessId() && !b.IsDeleted && b.BandLocationId == bandLocationId);
                    return Ok(_mapper.Map<IEnumerable<BillDto>>(entities));
                }
            }
            return NotFound();
        }

        // api/Search/Incomes/{bandLocationId}/Income/{name}
        [HttpGet("Incomes/{bandLocationId}/Income/{name}")]
        public async Task<IActionResult> SearchIncome([FromRoute] int? bandLocationId, [FromRoute] string? name)
        {
            if (bandLocationId != null)
            {
                if(name != null)
                {
                    IEnumerable<Income> entities = await _unitOfWork.Incomes.FindAllAsync(i => i.BusinessId == HttpContext.GetBusinessId() && !i.IsDeleted && i.BandLocationId == bandLocationId && i.Estatement == name);
                    return Ok(_mapper.Map<IEnumerable<IncomeDto>>(entities));
                }
                else
                {
                    IEnumerable<Income> entities = await _unitOfWork.Incomes.FindAllAsync(i => i.BusinessId == HttpContext.GetBusinessId() && !i.IsDeleted && i.BandLocationId == bandLocationId);
                    return Ok(_mapper.Map<IEnumerable<IncomeDto>>(entities));
                }
            }
            return NotFound();
        }



    }
}
