using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using BdTracker.Back.Settings;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;

namespace BdTracker.Back.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthSettings _authSettings;
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;

        public LoginController(UserManager<AppUser> userManager, IAuthSettings authSettings, ILogger<LoginController> logger, IMapper mapper)
        {
            _userManager = userManager;
            _authSettings = authSettings;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var identityUsr = await _userManager.FindByEmailAsync(request.Email);

            if (identityUsr is null)
            {
                var errorResponse = new ErrorResponse($"User with Email: {request.Email} not found.");
                _logger.LogError($"User with Email: {request.Email} not found.");
                return BadRequest(new List<ErrorResponse> { errorResponse });
            }

            var userRole = await _userManager.GetRolesAsync(identityUsr);

            if (await _userManager.CheckPasswordAsync(identityUsr, request.Password))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _authSettings.Issuer,
                    audience: _authSettings.Audience,
                    new List<Claim>
                    {
                        new Claim("Id", identityUsr.Id.ToString()),
                        new Claim("CompanyId", identityUsr.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, userRole.First().ToString())
                    },
                    signingCredentials: credentials);
                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(token);

                var loginResponse = _mapper.Map<LoginResponse>(identityUsr);
                loginResponse.Token = stringToken;

                identityUsr.LoginCounter += 1;
                var updateCounterResult = await _userManager.UpdateAsync(identityUsr);

                return Ok(loginResponse);
            }
            else
            {
                // TODO: add User lock after 10 unsuccessful attempt
                _logger.LogError($"User with Email {request.Email} enter wrong password");
                return Unauthorized(new List<ErrorResponse> { new ErrorResponse("You provide wrong password") });
            }
        }
    }
}