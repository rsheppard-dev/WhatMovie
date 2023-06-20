using Microsoft.Extensions.Options;
using WhatMovie.Models.Settings;
using WhatMovie.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatMovie.Models.Database;

namespace WhatMovie.Services
{
    public class SeedService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        
        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task ManageDataAsync()
        {
            await UpdateDatabaseAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCollections();
        }

        private async Task UpdateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        private async Task SeedRolesAsync()
        {
            // if any roles exist in database exit function
            if (_dbContext.Roles.Any()) return;

            var adminRole = _config.GetSection("DefaultCredentials")["Role"];

            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        private async Task SeedUsersAsync()
        {
            // if any users in database exit function
            if (_userManager.Users.Any()) return;
            
            var newUser = new IdentityUser()
            {
                Email = _config.GetSection("DefaultCredentials")["Email"],
                UserName = _config.GetSection("DefaultCredentials")["Email"],
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(newUser, _config.GetSection("DefaultCredentials")["Password"]);
            await _userManager.AddToRoleAsync(newUser, _config.GetSection("DefaultCredentials")["Role"]);
        }

        private async Task SeedCollections()
        {
            if (_dbContext.Collections!.Any()) return;

            var name = _appSettings?.WhatMovieSettings?.DefaultCollection?.Name!;
            var description = _appSettings?.WhatMovieSettings?.DefaultCollection?.Description!;

            _dbContext.Add(new Collection()
            {
                Name = name,
                Description = description
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}