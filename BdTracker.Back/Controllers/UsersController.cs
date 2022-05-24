using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BdTracker.Back.Services.Interfaces;
using BdTracker.Back.Validators;
using BdTracker.Shared.Constants;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Back.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly AddUserRequestValidator _addUserRequestValidator;
        private readonly IEmailService _emailService;

        public UsersController(UserManager<AppUser> userManager, IPasswordService passwordService, IEmailService emailService, ILogger<UsersController> logger, IMapper mapper, AddUserRequestValidator addUserRequestValidator)
        {
            _emailService = emailService;
            _userManager = userManager;
            _passwordService = passwordService;
            _logger = logger;
            _mapper = mapper;
            _addUserRequestValidator = addUserRequestValidator;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Owner,Admin")]
        public async Task<IActionResult> AddUserAsync(AddUserRequest request)
        {
            // get ID of person who create new User
            var creatorId = HttpContext?.User?.Claims.First(x => x.Type == "Id").Value;

            if (creatorId == null)
            {
                return BadRequest("You not log in.");
            }

            var creator = await _userManager.FindByIdAsync(creatorId);

            // validate request
            var validationResults = _addUserRequestValidator.Validate(request);

            if (!validationResults.IsValid)
            {
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(validationResults.Errors);
                return BadRequest(errors);
            }

            var user = _mapper.Map<AppUser>(request);
            user.CompanyId = creator.CompanyId;

            var userPassword = _passwordService.Generate();

            var addUserResult = await _userManager.CreateAsync(user, userPassword);

            if (!addUserResult.Succeeded)
            {
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(addUserResult.Errors);
                _logger.LogError(string.Join(',', errors.Select(x => x.Message)));
                return BadRequest(errors);
            }

            var addRoleToUserResult = await _userManager.AddToRoleAsync(user, AppConstants.UserRoleName);

            if (!addRoleToUserResult.Succeeded)
            {
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(addRoleToUserResult.Errors);
                _logger.LogError(string.Join(',', errors.Select(x => x.Message)));
                return BadRequest(errors);
            }

            // TODO: here must be send Email to registered user
            await _emailService.SendGreetings(user, userPassword);


            return Ok();
        }

    }
}