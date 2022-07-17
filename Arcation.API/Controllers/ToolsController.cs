using Arcation.API.Extentions;
using Arcation.Core;
using Arcation.Core.Models.ArcationModels.Main;
using Arcation.Core.ViewModels.ArcationViewModel;
using AutoMapper;
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
    public class ToolsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ToolsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // api/Tools: Create Tool
        [HttpPost]
        public async Task<IActionResult> CreateTool([FromBody] AddTool model)
        {
            if (ModelState.IsValid)
            {
                var isExist = await _unitOfWork.Tools.FindAsync(e => e.ToolName == model.ToolName);
                if (isExist == null)
                {
                    Tool newTool = new Tool
                    {
                        ToolName = model.ToolName,
                        Count = model.ToolCount
                    };

                    var result = await _unitOfWork.Tools.AddAsync(newTool);
                    if (result != null)
                    {
                        if (await _unitOfWork.Complete())
                        {
                            return Ok(_mapper.Map<ToolViewModel>(result));
                        }
                    }
                    return BadRequest();

                }
                return BadRequest("هذا الاسم موجود بالفعل");
            }
            return BadRequest(ModelState);
        }

        // api/Tools/{toolId}
        [HttpPut("{toolId}")]
        public async Task<IActionResult> EditTool([FromRoute] int? toolId, [FromBody] UpdateTool model)
        {
            if (toolId != null)
            {
                if (ModelState.IsValid)
                {
                    Tool queryTool = await _unitOfWork.Tools.FindAsync(e => e.ToolId == toolId && e.BusinessId == HttpContext.GetBusinessId());
                    if (queryTool != null)
                    {
                        queryTool.Count = model.ToolCount;
                        queryTool.ToolName = model.ToolName;

                        if (await _unitOfWork.Complete())
                        {
                            return Ok(_mapper.Map<ToolViewModel>(queryTool));
                        }
                        return BadRequest();
                    }
                    return NotFound();
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        // api/Tools
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tools = await _unitOfWork.Tools.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId());
            if (tools != null)
            {
                return Ok(_mapper.Map<IEnumerable<ToolViewModel>>(tools));
            }
            return NotFound();
        }

        // api/Tools/{toolId}
        [HttpGet("{toolID}")]
        public async Task<IActionResult> GetTool([FromRoute] int? toolId)
        {
            if (toolId != null)
            {
                var tool = await _unitOfWork.Tools.FindAllAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.ToolId == toolId);
                if (tool != null)
                {
                    return Ok(_mapper.Map<ToolViewModel>(tool));
                }
                return NotFound();
            }
            return NotFound();
        }

        // api/Tools/{toolId}
        [HttpDelete("{toolId}")]
        public async Task<IActionResult> DeleteTool([FromRoute] int? toolId)
        {
            if (toolId != null)
            {
                var tool = await _unitOfWork.Tools.FindAsync(e => e.BusinessId == HttpContext.GetBusinessId() && e.ToolId == toolId);
                if (tool != null)
                {
                    tool.IsDeleted = true;
                    if (await _unitOfWork.Complete())
                    {
                        return NoContent();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            return NotFound();
        }
    }
}
