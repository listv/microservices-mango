using IdentityModel;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializers;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        if (_roleManager.FindByNameAsync(StaticDetails.Admin).Result != null) return;

        _roleManager.CreateAsync(new IdentityRole(StaticDetails.Admin)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(StaticDetails.Customer)).GetAwaiter().GetResult();

        var adminUser = new ApplicationUser
        {
            UserName = "admin1@gmail.com",
            Email = "admin1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111111",
            FirstName = "Ben",
            LastName = "Admin"
        };

        _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, StaticDetails.Admin).GetAwaiter().GetResult();

        _userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new(JwtClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
            new(JwtClaimTypes.GivenName, $"{adminUser.FirstName}"),
            new(JwtClaimTypes.FamilyName, $"{adminUser.LastName}"),
            new(JwtClaimTypes.Role, StaticDetails.Admin)
        }).GetAwaiter().GetResult();

        var customerUser = new ApplicationUser
        {
            UserName = "customer1@gmail.com",
            Email = "customer1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111111",
            FirstName = "Ben",
            LastName = "Cust"
        };

        _userManager.CreateAsync(customerUser, "Customer123*").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(customerUser, StaticDetails.Customer).GetAwaiter().GetResult();

        var tempUser2 = _userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new(JwtClaimTypes.Name, $"{customerUser.FirstName} {customerUser.LastName}"),
            new(JwtClaimTypes.GivenName, $"{customerUser.FirstName}"),
            new(JwtClaimTypes.FamilyName, $"{customerUser.LastName}"),
            new(JwtClaimTypes.Role, StaticDetails.Customer)
        }).Result;
    }
}