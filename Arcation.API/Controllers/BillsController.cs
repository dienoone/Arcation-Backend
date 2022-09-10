using Arcation.API.Extentions;
using Arcation.API.Handler;
using Arcation.Core;
using Arcation.Core.Models;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BillsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageHandler _imageHandler;
        private readonly AppURL _appURL;
        // bandLocationId

        public BillsController(IUnitOfWork unitOfWork, IMapper mapper, IImageHandler imageHandler, IOptions<AppURL> appUrl)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageHandler = imageHandler;
            _appURL = appUrl.Value;
        }

        // api/Bills/{bandLocationId} => Get All Bills:
        [HttpGet("{bandLocationId}")]
        public async Task<IActionResult> GetAll([FromRoute] int? bandLocationId)
        {
            if(bandLocationId != null)
            {
                IEnumerable<Bill> entities = await _unitOfWork.Bills.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && !e.IsDeleted && e.BandLocationId == bandLocationId);
                if (entities != null)
                {
                    return Ok(_mapper.Map<IEnumerable<BillDto>>(entities));
                }
                return NoContent();
            }
            return NotFound();
        }

        // api/Bills/{bandLocationId}/Bill/{id} => Get Single Bill:
        [HttpGet("{bandLocationId}/Bill/{id}", Name = "GetBill")]
        public async Task<IActionResult> GetBill([FromRoute] int? bandLocationId, [FromRoute] int? id)
        {
            if (id != null)
            {
                Bill entity = await _unitOfWork.Bills.FindAsync(b => b.BillId == id && b.IsDeleted == false && b.BusinessId == HttpContext.GetUserId() && b.BandLocationId == bandLocationId);
                if (entity != null)
                {
                    return Ok(_mapper.Map<BillDto>(entity));
                }
                return NotFound();
            }
            return NotFound();

        }

        // api/Bills : => add Bill
        [HttpPost]
        public async Task<IActionResult> AddBill([FromBody] AddBillDto dto)
        {
            if (ModelState.IsValid)
            {
                Bill IsExist = await _unitOfWork.Bills.FindAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.BillCode == dto.BillCode && !e.IsDeleted);
                if (IsExist == null)
                {

                    Bill newBill = new Bill 
                    { 
                        BillCode = dto.BillCode,
                        BillDate = dto.BillDate,
                        BillNote = dto.BillNote,
                        BillPrice = dto.BillPrice,
                        BandLocationId = dto.BandLocationId,
                        IsDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = HttpContext.GetUserId(),
                        BusinessId = HttpContext.GetBusinessId()
                    };

                    //// ----- This Part Related To Photos ---------
                    //string photo = "photo";
                    //if (dto.BillPhoto != null)
                    //{
                    //    if (dto.BillPhoto.Length > 0)
                    //    {
                    //        photo = await _imageHandler.UploadImage(dto.BillPhoto);
                    //        if (photo == "Invalid image file") return BadRequest();
                    //    }
                    //}

                    //if (photo != "photo")
                    //{
                    //    newBill.BillPhoto = _appURL.AppUrl + photo;
                    //}
                    //else
                    //{
                    //    newBill.BillPhoto = null;
                    //}
                    //// --------------- End Region ----------------

                    await _unitOfWork.Bills.AddAsync(newBill);
                    if(await _unitOfWork.Complete())
                    {
                        return CreatedAtRoute("GetBill", new { controller = "Bills", id = newBill.BillId, bandLocationId = newBill.BandLocationId }, _mapper.Map<BillDto>(newBill));
                    }
                    return BadRequest("Error");
                    
                }
                return BadRequest("هذا الاسم موجود بالفعل");

            }
            return BadRequest(ModelState);
        }

        // api/Bills/{id} => Update Bill
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill([FromRoute] int? id, [FromBody] UpdateBillDto dto)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Bill queryBill = await _unitOfWork.Bills.FindAsync(b => b.BillId == id && b.BusinessId == HttpContext.GetBusinessId() && !b.IsDeleted);
                    if (queryBill != null)
                    {

                        queryBill.BillCode = dto.BillCode;
                        queryBill.BillDate = dto.BillDate;
                        queryBill.BillNote = dto.BillNote;
                        queryBill.BillPrice = dto.BillPrice;

                        //// ----- This Part Related To Photos ---------
                        //string photo = "photo";
                        //if (dto.BillPhoto != null)
                        //{
                        //    if (dto.BillPhoto.Length > 0)
                        //    {
                        //        photo = await _imageHandler.UploadImage(dto.BillPhoto);
                        //        if (photo == "Invalid image file") return BadRequest();
                        //    }
                        //}

                        //if (photo != "photo")
                        //{
                        //    queryBill.BillPhoto = _appURL.AppUrl + photo;
                        //}
                        //else
                        //{
                        //    queryBill.BillPhoto = null;
                        //}
                        //// --------------- End Region ----------------

                        if (await _unitOfWork.Complete())
                        {
                            return Ok(_mapper.Map<BillDto>(queryBill));
                        }

                        return BadRequest();
                        
                    }
                    return NotFound();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // api/Bills/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    Bill queryBill = await _unitOfWork.Bills.FindAsync(b => b.BillId == id);
                    if (queryBill != null)
                    {
                        
                        queryBill.IsDeleted = true;
                        if(await _unitOfWork.Complete())
                        {
                            return new NoContentResult();
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
