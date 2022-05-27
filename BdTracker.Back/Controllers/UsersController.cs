using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using BdTracker.Back.Services.Interfaces;
using BdTracker.Back.Validators;
using BdTracker.Shared.Constants;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;

namespace BdTracker.Back.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly AddUserRequestValidator _addUserRequestValidator;
        private readonly IEmailService _emailService;
        private readonly UpdateUserRequestValidator _updateUserRequestValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordService passwordService, IEmailService emailService,
            ILogger<UsersController> logger, IMapper mapper, AddUserRequestValidator addUserRequestValidator,
            UpdateUserRequestValidator updateUserRequestValidator)
        {
            _updateUserRequestValidator = updateUserRequestValidator;
            _emailService = emailService;
            _userManager = userManager;
            _passwordService = passwordService;
            _logger = logger;
            _mapper = mapper;
            _addUserRequestValidator = addUserRequestValidator;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserAsync(AddUserRequest request)
        {
            // get ID of person who create new User
            var creatorId = HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

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

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync(string id, UpdateUserRequest request)
        {
            var validationResults = await _updateUserRequestValidator.ValidateAsync(request);

            if (!validationResults.IsValid)
            {
                var errors = _mapper.Map<IEnumerable<ErrorResponse>>(validationResults.Errors);

                return BadRequest(errors);
            }

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var userCompanyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;


            if (HttpContext.User.IsInRole("SuperAdmin") ||
                HttpContext.User.IsInRole("Owner") ||
                HttpContext.User.IsInRole("Admin") ||
                userId == id)
            {
                AppUser? userToUpdate;

                // if user is SuperAdmin we will search in entire db else only in company
                if (HttpContext.User.IsInRole("SuperAdmin"))
                {
                    userToUpdate = await _userManager.FindByIdAsync(id);
                }
                else
                {
                    if (userCompanyId == null)
                    {
                        var errors = new List<ErrorResponse> { new ErrorResponse($"Your company is not found") };

                        return BadRequest(errors);
                    }

                    userToUpdate = await _userManager.Users.Where(x => x.CompanyId == userCompanyId).FirstOrDefaultAsync(x => x.Id == id);
                }

                if (userToUpdate == null)
                {
                    var errors = new List<ErrorResponse> { new ErrorResponse($"User with provide id {id} not found") };

                    return BadRequest(errors);
                }

                userToUpdate = _mapper.Map(request, userToUpdate);
                await _userManager.UpdateAsync(userToUpdate);
            }

            return NoContent();

        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            // If you a SuperAdmin you will return all users in application
            if (HttpContext.User.IsInRole("SuperAdmin"))
            {
                var allUsers = await _userManager.Users.ToListAsync();

                return Ok(_mapper.Map<IEnumerable<UserResponse>>(allUsers));
            }

            // companyId is for filter of users that will be returned
            var usersCompanyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;

            var users = await _userManager.Users.Where(u => u.CompanyId == usersCompanyId).ToListAsync();

            return Ok(_mapper.Map<IEnumerable<UserResponse>>(users));
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            // If you a SuperAdmin you will return all users in application
            if (HttpContext.User.IsInRole("SuperAdmin"))
            {
                var firstInAllUsers = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

                return Ok(_mapper.Map<UserResponse>(firstInAllUsers));
            }

            // companyId is for filter of users that will be returned
            var usersCompanyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;

            var companyUsers = await _userManager.Users.Where(u => u.CompanyId == usersCompanyId).ToListAsync();
            var firstInCompanyUsers = companyUsers.FirstOrDefault(x => x.Id == id);

            return Ok(_mapper.Map<UserResponse>(firstInCompanyUsers));
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var userCompanyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;


            if (HttpContext.User.IsInRole("SuperAdmin") ||
                HttpContext.User.IsInRole("Admin") ||
                userId == id)
            {
                AppUser? userToDelete;

                // if user is SuperAdmin we will search in entire db else only in company
                if (HttpContext.User.IsInRole("SuperAdmin"))
                {
                    userToDelete = await _userManager.FindByIdAsync(id);
                }
                else
                {
                    if (userCompanyId == null)
                    {
                        var errors = new List<ErrorResponse> { new ErrorResponse($"Your company is not found") };

                        return BadRequest(errors);
                    }

                    userToDelete = await _userManager.Users.Where(x => x.CompanyId == userCompanyId).FirstOrDefaultAsync(x => x.Id == id);
                }

                if (userToDelete == null)
                {
                    var errors = new List<ErrorResponse> { new ErrorResponse($"User with provide id {id} not found") };

                    return BadRequest(errors);
                }

                await _userManager.DeleteAsync(userToDelete);

                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("{id}/set-admin")]
        [Authorize(Roles = "SuperAdmin,Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetAdminRoleAsync(string id)
        {
            var companyId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;

            if (companyId == null)
            {
                var errors = new List<ErrorResponse> { new ErrorResponse("Your company does't exit") };
                return BadRequest(errors);
            }

            AppUser? user = null;

            if (HttpContext.User.IsInRole("SuperAdmin"))
            {
                user = await _userManager.FindByIdAsync(id);
            }
            else
            {
                user = await _userManager.Users.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync(x => x.Id == id);
            }

            if (user == null)
            {
                var errors = new List<ErrorResponse> { new ErrorResponse($"User with id {id} not found") };
                return BadRequest();
            }

            await _userManager.RemoveFromRoleAsync(user, AppConstants.UserRoleName);
            await _userManager.AddToRoleAsync(user, AppConstants.AdminRoleName);

            return NoContent();
        }
    }
}