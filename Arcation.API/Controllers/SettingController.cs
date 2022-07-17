using Arcation.API.Extentions;
using Arcation.Core;
using Arcation.Core.Interfaces;
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
    public class SettingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        public SettingController(IMapper mapper, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        // api/Setting
        [HttpGet]
        public async Task<IActionResult> GetAccountDetail()
        {
            var entity = await _accountRepository.GetUserDetail(HttpContext.GetUserId());
            return Ok(entity);
        }
    }
}
