using System.Net.Mime;
using System.Text.Json;
using AutoMapper;
using BdTracker.Back.Services.Interfaces;
using BdTracker.Back.Validators;
using BdTracker.Shared.Constants;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Back.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RegisterController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterController> _logger;
        private readonly RegisterOwnerRequestValidator _validator;

        public RegisterController(ICompanyService companyService, UserManager<AppUser> userManager, IMapper mapper, ILogger<RegisterController> logger, RegisterOwnerRequestValidator validator)
        {
            _companyService = companyService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterOwnerAsync(RegisterOwnerRequest request)
        {
            var validationResults = _validator.Validate(request);

            if (!validationResults.IsValid)
            {
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(validationResults.Errors);
                return BadRequest(errors);
            }

            // I think it's stupid check because it check is already at validation stage but why not
            if (string.IsNullOrEmpty(request.CompanyName))
            {
                var errors = new List<ErrorResponse> { new ErrorResponse($"You can't provide empty company name") };
                return BadRequest(errors);
            }

            if (_companyService.GetAllAsync().Result.FirstOrDefault(x => x.Name == request.CompanyName) != null)
            {
                var errors = new List<ErrorResponse> { new ErrorResponse($"Company with name {request.CompanyName} is already exists") };
                return BadRequest(errors);
            }

            var companyOwner = _mapper.Map<AppUser>(request);

            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.CompanyName,
                CompanyOwnerId = companyOwner.Id
            };

            companyOwner.CompanyId = company.Id;
            companyOwner.Company = company;

            var addUserResult = await _userManager.CreateAsync(companyOwner, request.Password);

            if (addUserResult.Succeeded)
            {
                var setRoleResult = await _userManager.AddToRoleAsync(companyOwner, AppConstants.OwnerRoleName);

                if (setRoleResult.Succeeded)
                {
                    return Ok(_mapper.Map<UserResponse>(companyOwner));
                }

                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(setRoleResult.Errors);
                return BadRequest(errors);
            }
            else
            {
                // todo: rewrite this return, because it is not correct
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(addUserResult.Errors);
                return BadRequest(errors);
            }
        }

    }
}