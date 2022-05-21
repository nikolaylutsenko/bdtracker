using Microsoft.AspNetCore.Identity;

namespace BdTracker.Shared.Entities;

public class AppUser : IdentityUser<string>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDay { get; set; }
    public string? PositionName { get; set; }
    public string? CompanyId { get; set; }
    public Sex Sex { get; set; }

    public virtual ICollection<IdentityUserClaim<string>>? Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<string>>? Logins { get; set; }
    public virtual ICollection<IdentityUserToken<string>>? Tokens { get; set; }
    public virtual ICollection<AppUserAppRole>? UserRoles { get; set; }
    public virtual Company? Company { get; set; }
}

public class AppRole : IdentityRole
{
    public virtual ICollection<AppUserAppRole>? UserRoles { get; set; }
}

public class AppUserAppRole : IdentityUserRole<string>
{
    public virtual AppUser? User { get; set; }
    public virtual AppRole? Role { get; set; }
}
