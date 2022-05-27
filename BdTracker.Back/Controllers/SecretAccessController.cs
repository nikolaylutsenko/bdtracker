using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BdTracker.Back.Settings;
using BdTracker.Shared.Models.Request;

namespace BdTracker.Back.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/secret")]
    public class SecretAccessController : ControllerBase
    {
        private readonly ISecretAccessSettings _secretAccessSettings;
        private readonly IConnectionStringSettings _connectionStringSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<SecretAccessController> _logger;

        public SecretAccessController(ISecretAccessSettings secretAccessSettings, IConnectionStringSettings connectionStringSettings,
        IWebHostEnvironment webHostEnvironment, ILogger<SecretAccessController> logger)
        {
            _secretAccessSettings = secretAccessSettings;
            _connectionStringSettings = connectionStringSettings;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpPost("db-file")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetDBFileAsync(DbSecretKeyRequest request)
        {
            if (_secretAccessSettings.Key != request.Key ||
                _secretAccessSettings.Value != request.Value)
            {
                return Unauthorized("Access denied!");
            }

            // TODO: its bead, better re-wright it
            var connectionString = "App_Data\\bdtdatabase.db";
            var env = _webHostEnvironment.ContentRootPath;
            var filePath = string.Concat(env, connectionString);

            var data = await System.IO.File.ReadAllBytesAsync(filePath);

            return new FileContentResult(data, "application/database");
        }
    }
}