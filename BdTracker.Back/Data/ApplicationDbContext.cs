using BdTracker.Shared;
using BdTracker.Shared.Constants;
using BdTracker.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BdTracker.Back.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string,
        IdentityUserClaim<string>, AppUserAppRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            base.OnModelCreating(modelBuider);

            modelBuider.Entity<AppUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuider.Entity<AppRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuider.Entity<AppRole>().HasData(new AppRole
            {
                Id = "58f3dea3-67eb-4284-b4bd-e4504d8e523e",
                Name = AppConstants.SuperAdminRoleName,
                NormalizedName = AppConstants.SuperAdminRoleName.ToUpper(),
                ConcurrencyStamp = "58f3dea3-67eb-4284-b4bd-e4504d8e523e"
            });

            modelBuider.Entity<AppRole>().HasData(new AppRole
            {
                Id = "6dbb9259-ba9f-41f0-bc67-af39787f7e0a",
                Name = AppConstants.OwnerRoleName,
                NormalizedName = AppConstants.OwnerRoleName.ToUpper(),
                ConcurrencyStamp = "6dbb9259-ba9f-41f0-bc67-af39787f7e0a"
            });

            modelBuider.Entity<AppRole>().HasData(new AppRole
            {
                Id = "0a26e36f-1626-4298-9a97-34a8c4118e08",
                Name = AppConstants.AdminRoleName,
                NormalizedName = AppConstants.AdminRoleName.ToUpper(),
                ConcurrencyStamp = "0a26e36f-1626-4298-9a97-34a8c4118e08"
            });

            modelBuider.Entity<AppRole>().HasData(new AppRole
            {
                Id = "dd62e685-29cf-4dd7-b59b-e44022d88d29",
                Name = AppConstants.UserRoleName,
                NormalizedName = AppConstants.UserRoleName.ToUpper(),
                ConcurrencyStamp = "dd62e685-29cf-4dd7-b59b-e44022d88d29"
            });

            modelBuider.Entity<AppUser>().HasData(new AppUser
            {
                Id = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                UserName = "SuperAdmin",
                Email = "super.admin@bdtracker.com",
                EmailConfirmed = true,
                ConcurrencyStamp = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                NormalizedEmail = "super.admin@bdtracker.com".ToUpper(),
                NormalizedUserName = AppConstants.SuperAdminRoleName.ToUpper(),
                PasswordHash = "AQAAAAEAACcQAAAAEO1PQ5JXbydpkx3TOGqWM8uW1z3dMbYFjhRqDXzTAQXzxdFyF5f6VDdMmnJk8z44TA==",
                SecurityStamp = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
                BirthDay = DateTime.Parse("2022-01-22T00:00:00"),
                Name = "Super",
                Surname = "Admin",
                PositionName = "Super Admin",
                CompanyId = "506c4655-0053-414e-bb8f-9612876ab2a1",
                Sex = Sex.Other
            });

            modelBuider.Entity<AppUser>().ToTable("AppUser");
            modelBuider.Entity<AppUser>()
                .Property(x => x.UserName)
                .IsRequired(false);

            modelBuider.Entity<AppRole>().ToTable("AppRole");

            modelBuider.Entity<AppUserAppRole>().ToTable("AppUserAppRole");

            modelBuider.Entity<AppUserAppRole>().HasData(new AppUserAppRole
            {
                RoleId = "58f3dea3-67eb-4284-b4bd-e4504d8e523e",
                UserId = "25d733fa-b5ce-41fe-a868-beea7723a3e5"
            });

            modelBuider.Entity<Company>()
                .HasMany(emp => emp.Employees)
                .WithOne(company => company.Company);

            modelBuider.Entity<Company>()
                .HasData(new Company
                {
                    Id = "506c4655-0053-414e-bb8f-9612876ab2a1",
                    Name = "BdTracker",
                    CompanyOwnerId = "25d733fa-b5ce-41fe-a868-beea7723a3e5"
                });
        }

        public DbSet<Company>? Companies { get; set; }
    }
}