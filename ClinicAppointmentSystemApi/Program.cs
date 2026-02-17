
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Persistence.DataSeed;
using ClinicAppointment.Persistence.DbContexts;
using ClinicAppointment.Persistence.IdentityData.DataSeed;
using ClinicAppointment.Persistence.IdentityData.DbContexts;
using ClinicAppointment.Persistence.Repositories;
using ClinicAppointment.Service;
using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Service.IdentityModels;
using ClinicAppointmentSystemApi.BackgroundJobs;
using ClinicAppointmentSystemApi.Extension;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentSystemApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "اكتب التوكن هنا بالشكل التالي: Bearer {your token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });



            builder.Services.AddDbContext<ClinicAppointmentsDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddDbContext<ClinicAppointmentsIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });




            builder.Services.AddHangfire(config =>
             config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();


            builder.Services.AddAutoMapper(typeof(ServiceAssemplyRefrence).Assembly);

            builder.Services.AddKeyedScoped<IDataIntializer, DataIntializer>("Default");
            builder.Services.AddKeyedScoped<IDataIntializer, IdentityDataIntializer>("Identity");


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<ISpecializationService, SpecializationService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IBackgroundJobService, HangfireJobService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


            builder.Services.AddIdentityCore<ApplicationUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ClinicAppointmentsIdentityDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                      ValidAudience = builder.Configuration["JWTOptions:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!)
          ),
                  };
              });





            var app = builder.Build();


            app.UseHangfireDashboard("/hangfire");


            #region UpdateDb_Pending_Migrations And DataSeeding

            await app.MigrateDataBaseAsync();
            await app.MigrateIdentityDataBaseAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();

            # endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                });

                app.UseHttpsRedirection();


                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                await app.RunAsync();
            }
        }
    }
}

