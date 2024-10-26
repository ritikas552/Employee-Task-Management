using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EmployeeTaskManagement;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<EmployeeTaskManagementDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<User, Roles>()
            .AddEntityFrameworkStores<EmployeeTaskManagementDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<IAdminServices, AdminServices>();
        builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
        builder.Services.AddScoped<LoggedUserService, LoggedUserService>();
        builder.Services.AddScoped<ITokenBlacklistService, TokenBlackListService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("http://localhost:8080") // Replace with your frontend URL
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        //builder.Services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Task Management", Version = "v1" });

        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.Http,
        //        Scheme = "Bearer",
        //        BearerFormat = "JWT",
        //        In = ParameterLocation.Header,
        //        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'",
        //    });

        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "Bearer"
        //                }
        //            },
        //            new string[] {}
        //        }
        //    });
        //});

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    if (await context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>().IsTokenBlacklistedAsync(token))
                    {
                        context.Fail("Token is blacklisted.");
                    }
                }
            };
        });

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        //builder.Services.AddControllers();
        //builder.Services.AddEndpointsApiExplorer();


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            Console.WriteLine("Request Headers: " + string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));
            await next.Invoke();
        });

        app.MapControllers();

        app.Run();
    }
}
