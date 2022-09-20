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
    [Authorize(Roles = "Admin, User")]
    public class EmployeeTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // api/employeetypes :
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<EmployeeType> entities = await _unitOfWork.EmployeeTypes.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.IsDeleted == false);
            if(entities != null)
            {
                var types = _mapper.Map<IEnumerable<EmployeeTypeDto>>(entities);
                foreach(var type in types)
                {
                    type.Count = await _unitOfWork.Employees.CountAsync(e => e.TypeId == type.Id && e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted);
                }
                return Ok(types);
            }
            return NoContent();
            
        }

        // api/employeetypes/{id}
        [HttpGet("{id}", Name = "GetEmployeeType")]
        public async Task<IActionResult> GetEmployeeType([FromRoute] int? id)
        {
            if (id != null)
            {
                EmployeeType entity = await _unitOfWork.EmployeeTypes.FindAsync(c => c.Id == id && c.IsDeleted == false && c.BusinessId == HttpContext.GetBusinessId());
                if (entity != null)
                {
                    return Ok(_mapper.Map<EmployeeTypeDto>(entity));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/employeetypes :
        [HttpPost]
        public async Task<IActionResult> AddEmployeeType([FromBody] AddEmployeeTypeDto model)
        {
            if (ModelState.IsValid)
            {
                EmployeeType newType = _mapper.Map<EmployeeType>(model);
                newType.IsDeleted = false;
                newType.CreatedAt = DateTime.UtcNow;
                newType.CreatedBy = HttpContext.GetUserId();
                newType.BusinessId = HttpContext.GetBusinessId();

                var isAdded = await _unitOfWork.EmployeeTypes.AddAsync(newType);
                if (await _unitOfWork.Complete() && isAdded != null)
                {
                    return CreatedAtRoute("GetEmployeeType", new { controller = "EmployeeTypes", id = newType.Id }, _mapper.Map<EmployeeTypeDto>(newType));
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        // api/employeetypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBand([FromRoute] int? id, [FromBody] UpdateEmployeeTypeDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    EmployeeType queryType = await _unitOfWork.EmployeeTypes.FindAsync(c => c.Id == id);
                    if (queryType != null)
                    {
                        queryType.Type = model.Type;
                        await _unitOfWork.Complete();

                        return Ok(_mapper.Map<EmployeeTypeDto>(queryType));
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/employeetypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBand([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    EmployeeType queryType = await _unitOfWork.EmployeeTypes.FindAsync(c => c.Id == id);
                    if (queryType != null)
                    {
                        queryType.IsDeleted = true;
                        _unitOfWork.EmployeeTypes.Update(queryType);
                        await _unitOfWork.Complete();

                        return NoContent();
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }


    }
}
