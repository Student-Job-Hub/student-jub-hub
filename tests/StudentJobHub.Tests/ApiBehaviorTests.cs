using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentJobHub.Api.Data;
using StudentJobHub.Api.DTOs.Auth;
using StudentJobHub.Api.DTOs.Jobs;
using StudentJobHub.Api.Models;
using StudentJobHub.Api.Services;

namespace StudentJobHub.Tests;

public class ApiBehaviorTests
{
    private static async Task<(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, AuthService AuthService, JobService JobService)> CreateServicesAsync()
    {
        var databaseName = Guid.NewGuid().ToString();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDataProtection();
        services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<ApplicationDbContext>();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var authService = new AuthService(userManager, new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "StudentJobHub_Test_Key_1234567890_ABCDEFGH1234567890",
            ["Jwt:Issuer"] = "StudentJobHub",
            ["Jwt:Audience"] = "StudentJobHubAudience"
        }).Build());

        var jobService = new JobService(context);

        await EnsureRolesAsync(roleManager);

        return (context, userManager, authService, jobService);
    }

    private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var roleName in new[] { "Student", "Business", "Lecturer" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    [Fact]
    public async Task RegisterAsync_CreatesUserAndGeneratesToken()
    {
        var (_, _, authService, _) = await CreateServicesAsync();

        var result = await authService.RegisterAsync(new RegisterDto
        {
            FullName = "Student User",
            Email = "student@test.com",
            Password = "Password123!",
            Role = "Student",
            University = "Test University"
        });

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_Fails()
    {
        var (_, _, authService, _) = await CreateServicesAsync();

        await authService.RegisterAsync(new RegisterDto
        {
            FullName = "Student User",
            Email = "student@test.com",
            Password = "Password123!",
            Role = "Student"
        });

        var result = await authService.RegisterAsync(new RegisterDto
        {
            FullName = "Another User",
            Email = "student@test.com",
            Password = "Password123!",
            Role = "Student"
        });

        Assert.False(result.Success);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        var (_, _, authService, _) = await CreateServicesAsync();

        await authService.RegisterAsync(new RegisterDto
        {
            FullName = "Student User",
            Email = "student@test.com",
            Password = "Password123!",
            Role = "Student"
        });

        var result = await authService.LoginAsync(new LoginDto
        {
            Email = "student@test.com",
            Password = "Password123!"
        });

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task JobService_CreateAndGetById_Succeeds()
    {
        var (context, _, _, jobService) = await CreateServicesAsync();

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "poster@test.com",
            Email = "poster@test.com",
            FullName = "Poster User"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var created = await jobService.CreateAsync(new CreateJobDto
        {
            Title = "Test Job",
            Description = "Description",
            Requirements = "Requirements",
            Budget = 100,
            Deadline = DateTime.UtcNow.AddDays(7)
        }, user.Id);

        var retrieved = await jobService.GetByIdAsync(created.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("Test Job", retrieved!.Title);
        Assert.Equal(user.Id, created.PostedById);
        Assert.NotSame(created, retrieved);
    }
}
