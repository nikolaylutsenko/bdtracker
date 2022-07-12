using BdTracker.Back.Services.Interfaces;
using PasswordGenerator;

namespace BdTracker.Back.Services;

public class PasswordService : IPasswordService
{
    private readonly Password password;
    public PasswordService()
    {
        password = new Password(
            includeLowercase: true,
            includeUppercase: true,
            includeNumeric: true,
            includeSpecial: true,
            passwordLength: 8
        );
    }

    public string Generate()
    {
        return password.Next();
    }
}