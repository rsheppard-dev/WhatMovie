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
        
        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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

            var adminRole = _appSettings.WhatMovieSettings?.DefaultCredentials?.DefaultRole;

            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        private async Task SeedUsersAsync()
        {
            // if any users in database exit function
            if (_userManager.Users.Any()) return;

            var credentials = _appSettings?.WhatMovieSettings?.DefaultCredentials!;
            
            var newUser = new IdentityUser()
            {
                Email = credentials.DefaultEmail,
                UserName = credentials.DefaultEmail,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(newUser, credentials.DefaultPassword);
            await _userManager.AddToRoleAsync(newUser, credentials.DefaultRole);
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