using BdTracker.Shared.Entities;

namespace BdTracker.Back.Services.Interfaces;

public interface IEmailService
{
    Task SendGreetings(AppUser user, string password);
}