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

    public async Task SendGreetings(AppUser user, string password)
    {
        await Task.Factory.StartNew(() => _logger.LogWarning($"{user.Name} - {password}"));
    }
}