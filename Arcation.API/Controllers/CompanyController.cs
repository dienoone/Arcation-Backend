using Arcation.API.Extentions;
using Arcation.API.Handler;
using Arcation.Core;
using Arcation.Core.Interfaces.ArcationInterfaces;
using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageHandler _imageHandler;
        private readonly AppURL _appURL;

        public CompanyController(IUnitOfWork unitOfWork, IMapper mapper, IImageHandler imageHandler, IOptions<AppURL> appUrl)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageHandler = imageHandler;
            _appURL = appUrl.Value;
        }
 
        // api/Company
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.Companies.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.IsDeleted == false);
            return Ok(_mapper.Map<IEnumerable<CompanyViewModel>>(entities));
        }

        // api/Company/{id}
        [HttpGet("{id}", Name ="GetCompany")]
        public async Task<IActionResult> GetCompany([FromRoute] int? id)
        {
            if(id != null)
            {
                var entity = await _unitOfWork.Companies.FindAsync(c => c.Id == id && c.IsDeleted == false && c.BusinessId == HttpContext.GetBusinessId());
                if (entity != null)
                {   
                    return Ok(_mapper.Map<CompanyViewModel>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/Company : => add company
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromForm] AddCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {

                var IsExist = await _unitOfWork.Companies.FindAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.Name == model.Name);
                if(IsExist == null)
                {

                    Company newCompany = new Company
                    {
                        Name = model.Name,
                        IsDeleted = false,
                        BusinessId = HttpContext.GetBusinessId(),
                        CreatedBy = HttpContext.GetUserId(),
                        CreatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.Companies.AddAsync(newCompany);

                    if(await _unitOfWork.Complete())
                    {
                        return CreatedAtRoute("GetCompany", new { controller = "Company", id = newCompany.Id }, _mapper.Map<CompanyViewModel>(newCompany));
                    }
                    return BadRequest();
                    
                }
                return BadRequest("هذا الاسم موجود بالفعل");
                
            }
            return BadRequest(ModelState);
        }

        // api/Company/{id} => Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] int? id, [FromForm] UpdateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Company queryCompany = await _unitOfWork.Companies.FindAsync(c => c.Id == id);
                    if (queryCompany != null)
                    {
                        queryCompany.Name = model.Name;
                        return Ok(_mapper.Map<CompanyViewModel>(queryCompany));

                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/Company/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Company queryCompany = await _unitOfWork.Companies.FindAsync(c => c.Id == id && c.BusinessId == HttpContext.GetBusinessId());
                    if (queryCompany != null)
                    {
                        queryCompany.IsDeleted = true;
                        if(await _unitOfWork.Complete())
                        {
                            return NoContent();
                        }
                        return BadRequest();
                        
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

    }
}
