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
    [Authorize(Roles = "Admin")]
    public class ExtractController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ExtractController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        #region Main:

        // api/extract/{bandLocationId}
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll(int? bandLocationId)
        {
            if (bandLocationId != null)
            {
                var entities = await _unitOfWork.Extracts.FindAllAsync(e => e.BandLocationId == bandLocationId && e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted);
                if (entities != null)
                {
                    return Ok(_mapper.Map<IEnumerable<AllExtracts>>(entities));
                }
                return NoContent();
            }
            return NotFound();
        }

        // api/extract/Single/{extractId}
        [HttpGet("Single/{extractId}")]
        public async Task<IActionResult> GetExtract(int? extractId)
        {
            if (extractId != null)
            {
                Extract entity = await _unitOfWork.Extracts.FindAsync(e => e.ExtractId == extractId && e.BusinessId ==HttpContext.GetBusinessId() && !e.IsDeleted);
                if (entity != null)
                {
                    return Ok(_mapper.Map<AllExtracts>(entity));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/extract/{bandLocationId} => create extract:
        [HttpPost("{bandLocationId}")]
        public async Task<IActionResult> CreateExtract([FromRoute] int? bandLocationId, [FromBody] AddExtract model)
        {
            if (bandLocationId != null)
            {
                Extract isExist = await _unitOfWork.Extracts.FindAsync(e => e.ExtractName == model.ExtractName && e.BandLocationId == bandLocationId && e.BusinessId == HttpContext.GetBusinessId());
                if (isExist == null)
                {
                    Extract newExtract = new Extract
                    {
                        ExtractName = model.ExtractName,
                        TotalPrice = model.TotalPrice,
                        BusinessId = HttpContext.GetBusinessId(),
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = HttpContext.GetUserId(),
                        BandLocationId = (int)bandLocationId
                    };

                    Extract result = await _unitOfWork.Extracts.AddAsync(newExtract);

                    if (result != null && await _unitOfWork.Complete())
                    {
                        Extract entity = await _unitOfWork.Extracts.GetExtractAsync(result.ExtractId, HttpContext.GetBusinessId());
                        if (entity != null)
                        {
                            return Ok(_mapper.Map<AllExtracts>(entity));
                        }
                        return NotFound();
                    }
                    return BadRequest();

                }
                return BadRequest("هذا الاسم موجود بالفعل");
            }
            return NotFound();
        }

        // api/extract/{extractId}
        [HttpPut("{extractId}")]
        public async Task<IActionResult> UpdateExtract([FromRoute] int? extractId, [FromBody] UpdateExtract model)
        {
            if (extractId != null)
            {
                Extract queryExtract = await _unitOfWork.Extracts.FindAsync(e => e.ExtractId == extractId && e.BusinessId == HttpContext.GetBusinessId());
                if (queryExtract != null)
                {

                    queryExtract.ExtractName = model.ExtractName;
                    queryExtract.TotalPrice = model.TotalPrice;
                    await _unitOfWork.Complete();
                    Extract entity = await _unitOfWork.Extracts.GetExtractAsync(extractId, HttpContext.GetBusinessId());
                    if (entity != null)
                    {
                        return Ok(_mapper.Map<AllExtracts>(entity));
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/extract/{extractId}
        [HttpDelete("{extractId}")]
        public async Task<IActionResult> DeleteExtract([FromRoute] int? extractId)
        {
            if (extractId != null)
            {
                Extract queryExtract = await _unitOfWork.Extracts.FindAsync(e => e.ExtractId == extractId && e.BusinessId == HttpContext.GetBusinessId());
                if (queryExtract != null)
                {
                    queryExtract.IsDeleted = true;
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                return NotFound();
            }
            return NotFound();
        }

        #endregion

        #region Rows:
        // api/extract/rows/{extractId}
        [HttpPost("Rows/{extractId}")]
        public async Task<IActionResult> CreateRow([FromRoute]int? extractId, [FromBody] AddExtractRow model)
        {
            if(extractId != null)
            {
                ExtractRow isExist = await _unitOfWork.ExtractRows.FindAsync(e => e.ExtractId == extractId && e.Estatement == model.Estatement && e.BusinessId == HttpContext.GetBusinessId());
                if(isExist == null)
                {
                    ExtractRow newExtractRow = new ExtractRow
                    {
                        ExtractId = (int)extractId,
                        Estatement = model.Estatement,
                        EstatementUnite = model.EstatementUnite,
                        EstatementUnitePrice = model.EstatementUnitePrice,
                        Note = model.Note,
                        Percentage = model.Percentage,
                        Quantity = model.Quantity,
                        BusinessId = HttpContext.GetBusinessId(),
                        CreatedBy = HttpContext.GetUserId(),
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    newExtractRow.TotalPrice = model.EstatementUnitePrice * model.Quantity * (model.Percentage / 100);

                    ExtractRow result = await _unitOfWork.ExtractRows.AddAsync(newExtractRow);

                    if(result != null && await _unitOfWork.Complete())
                    {
                        Extract entity = await _unitOfWork.Extracts.GetExtractAsync(extractId, HttpContext.GetBusinessId());
                        if (entity != null)
                        {
                            return Ok(_mapper.Map<AllExtracts>(entity));
                        }
                        return NotFound();
                    }
                    return BadRequest();
                }
                return BadRequest("هذا البيان موجود بالفعل");
            }
            return NotFound();
        }
        // api/extract/rows/{extractRowId}
        [HttpPut("Rows/{extractRowId}")]
        public async Task<IActionResult> UpdateRow([FromRoute] int? extractRowId, [FromBody] UpdateExtractRow model)
        {
            if(extractRowId != null)
            {
                if (ModelState.IsValid)
                {
                    ExtractRow queryExtractRow = await _unitOfWork.ExtractRows.FindAsync(e => e.ExtractRowId == extractRowId && e.BusinessId == HttpContext.GetBusinessId());
                    if (queryExtractRow != null)
                    {
                        queryExtractRow.Estatement = model.Estatement;
                        queryExtractRow.EstatementUnite = model.EstatementUnite;
                        queryExtractRow.EstatementUnitePrice = model.EstatementUnitePrice;
                        queryExtractRow.Note = model.Note;
                        queryExtractRow.Percentage = model.Percentage;
                        queryExtractRow.Quantity = model.Quantity;
                        queryExtractRow.TotalPrice = model.EstatementUnitePrice * model.Quantity * (model.Percentage / 100);
                        await _unitOfWork.Complete();

                        Extract entity = await _unitOfWork.Extracts.GetExtractAsync(queryExtractRow.ExtractId, HttpContext.GetBusinessId());
                        if (entity != null)
                        {
                            return Ok(_mapper.Map<AllExtracts>(entity));
                        }
                        return NotFound();

                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }
        // api/extract/rows/{extractRowId}
        [HttpDelete("Rows/{extractRowId}")]
        public async Task<IActionResult> DeleteRow(int? extractRowId)
        {
            if(extractRowId != null)
            {
                ExtractRow queryExtractRow = await _unitOfWork.ExtractRows.FindAsync(e => e.ExtractRowId == extractRowId && e.BusinessId == HttpContext.GetBusinessId());
                if(queryExtractRow != null)
                {
                    queryExtractRow.IsDeleted = false;
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                return NotFound();
            }
            return NotFound();
        }
        #endregion
    }
}
