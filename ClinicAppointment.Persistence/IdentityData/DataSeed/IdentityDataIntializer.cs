using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Service.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.IdentityData.DataSeed
{
    public class IdentityDataIntializer : IDataIntializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataIntializer> _logger;

        public IdentityDataIntializer(UserManager<ApplicationUser> userManager ,
            RoleManager<IdentityRole> roleManager ,ILogger<IdentityDataIntializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }



        public async Task InitializeDataAsync()
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!await _roleManager.RoleExistsAsync("Patient"))
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));

                if (!await _roleManager.RoleExistsAsync("Doctor"))
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));

                if (!_userManager.Users.Any())
                {
                    var User = new ApplicationUser()
                    {
                       DisplayName = "Hamza Oraby",
                       Email = "Hamza44@gmail.com",
                       UserName = "HamzaOraby",
                       PhoneNumber = "01092772908"
                    };

                    await _userManager.CreateAsync(User,"P@ssw0rd");
                    await _userManager.AddToRoleAsync(User,"Admin");


                }


            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error While Seeding Identity DataBase Message{ex.Message}");
            }

        }
    }
}
