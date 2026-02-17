using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Entites.PatientModule;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Service.IdentityModels;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<Result<CurrentUserDTO>> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Error.NotFound("User.NotFound");


            return new CurrentUserDTO(user.Email!, user.DisplayName);
        }

        public async Task<Result<UserDTO>> LoginAsyns(LoginDto loginDto)
        {
       
            var User = await _userManager.FindByEmailAsync(loginDto.Email);   

             if (User is null)
                return Error.InValidCerdentials("User.InValidCerdentils");

            var IsPassword= await _userManager.CheckPasswordAsync(User,loginDto.Password);

            if (!IsPassword)
                return Error.InValidCerdentials("User.InValidCerdentils");

            // Read A Role => Patient Or Doctor          

            var Token = await CreateTokenAsync(User);

            return new UserDTO(User.Email!,User.DisplayName, Token);


        }

        public async Task<Result<UserDTO>> RegisterDoctorAsync(RegisterDoctorDTO dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Error.Validation("Email.Exists", "Email already exists");

            var specialization = await _unitOfWork
                .GetRepository<Specialization, int>()
                .GetByIdAsync(dto.SpecializationId);

            if (specialization is null)
                return Error.NotFound("Specialization.NotFound");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.UserName,
                DisplayName = dto.DisplayName,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                Name = dto.DisplayName,
                Bio = dto.Bio,
                ConsultationFees = dto.Fees,
                SpecializationId = dto.SpecializationId,
                IdentityUserId = user.Id
            };

            await _unitOfWork.GetRepository<Doctor, int>().AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            var Token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, Token);
        }

        public async Task<Result<UserDTO>> RegisterPatientAsync(RegisterPatientDTO dto)
        {

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Error.Validation("Email.Exists", "Email already exists");


            var User = new ApplicationUser()
            {
              UserName=dto.UserName,
              Email=dto.Email,
              PhoneNumber=dto.PhoneNumber,
              DisplayName=dto.DisplayName,
            };

            var IdentityResult = await _userManager.CreateAsync(User,dto.Password);

              if (!(IdentityResult.Succeeded))
                return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();



            await _userManager.AddToRoleAsync(User, "Patient");

            var Patient = new Patient()
            {
                FullName = User.DisplayName,
                PhoneNumber = User.PhoneNumber,
                IdentityUserId = User.Id
               // Connection Patient as Bussines Table With Patient As User
            };

          await _unitOfWork.GetRepository<Patient,int>().AddAsync(Patient);  
              
          await _unitOfWork.SaveChangesAsync();
         

                var Token = await CreateTokenAsync(User);
                return new UserDTO(User.Email, User.DisplayName, Token);


        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Email, user.Email!),
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
              new Claim(ClaimTypes.NameIdentifier, user.Id) // مهم للربط مع Patient/Doctor
             };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWTOptions:SecretKey"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }






    }
}
