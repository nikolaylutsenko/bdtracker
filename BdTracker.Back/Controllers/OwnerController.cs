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
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class OwnerController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RegisterOwnerRequestValidator _validator;

        public OwnerController(ICompanyService companyService, UserManager<AppUser> userManager, IMapper mapper, RegisterOwnerRequestValidator validator)
        {
            _companyService = companyService;
            _userManager = userManager;
            _mapper = mapper;
            _validator = validator;

        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> RegisterOwnerAsync(RegisterOwnerRequest request)
        {
            var validationResults = _validator.Validate(request);

            if (!validationResults.IsValid)
            {
                return Results.BadRequest(validationResults.Errors);
            }

            // I think it's stupid check because it check is already at validation stage but why not
            if (string.IsNullOrEmpty(request.CompanyName))
            {
                return Results.BadRequest($"You can't provide empty company name");
            }

            if (_companyService.GetAllAsync().Result.FirstOrDefault(x => x.Name == request.CompanyName) != null)
            {
                return Results.BadRequest($"Company with name {request.CompanyName} is already exists");
            }

            var companyOwner = _mapper.Map<AppUser>(request);

            // create and add company into db
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
                var setRoleResult = await _userManager.AddToRoleAsync(companyOwner, AppConstants.SuperAdminRoleName);

                if (setRoleResult.Succeeded)
                {
                    return Results.Ok(JsonSerializer.Serialize<EmployeeResponse>(_mapper.Map<EmployeeResponse>(companyOwner)));
                }

                // todo: rewright this to use standart exception
                return Results.Problem(string.Join(',', setRoleResult.Errors.Select(x => x.Description)));
            }
            else
            {
                // remove company from db
                //await _companyService.DeleteAsync(company.Id);

                // todo: rewrite this return, because it is not correct
                // todo: rewrite for use standart error response
                return Results.BadRequest(string.Join(',', addUserResult.Errors.Select(x => x.Description)));
            }
        }

    }
}