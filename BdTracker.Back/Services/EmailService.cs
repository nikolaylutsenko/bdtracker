using BdTracker.Back.Services.Interfaces;
using BdTracker.Shared.Entities;

namespace BdTracker.Back.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendGreetings(AppUser user, string password)
    {
        return Task.Factory.StartNew(() => _logger.LogDebug(user.Name, password));
    }
}