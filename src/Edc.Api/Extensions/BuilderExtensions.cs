using System.Text;
using Edc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Edc.Api.Extensions;

public static class BuilderExtension {
    public static void AddConfiguration(this WebApplicationBuilder builder) {
        Config.Database.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;

        Config.Secrets.ApiSecretKey = builder.Configuration.GetSection("SecretsConfig").GetValue<string>("ApiSecretKey") ?? String.Empty;
        Config.Secrets.TokenPrivateKey = builder.Configuration.GetSection("SecretsConfig").GetValue<string>("TokenPrivateKey") ?? String.Empty;
        Config.Secrets.PasswordSalt = builder.Configuration.GetSection("SecretsConfig").GetValue<string>("PasswordSalt") ?? String.Empty;

        Config.Email.DefaultFromEmail = builder.Configuration.GetSection("EmailConfig").GetValue<string>("DefaultFromEmail") ?? String.Empty;
        Config.Email.DefaultFromName = builder.Configuration.GetSection("EmailConfig").GetValue<string>("DefaultFromName") ?? String.Empty;

        Config.SendGridClient.ApiKey = builder.Configuration.GetSection("SendGridClientConfig").GetValue<string>("ApiKey") ?? String.Empty;

    }

    public static void AddDbContext(this WebApplicationBuilder builder) {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
        builder.Services.AddDbContext<EdcDbContext>(x =>
            x.UseMySql(
                Config.Database.ConnectionString,
                serverVersion,
                b => b.MigrationsAssembly("Edc.Api")
            )
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors());
    }

    public static void AddAuthentication(this WebApplicationBuilder builder) {
        builder.Services
            .AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.Secrets.TokenPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddAuthorization(x => {
            x.AddPolicy("User", policy => policy.RequireRole("User"));
            x.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });
    }

    public static void AddMediatR(this WebApplicationBuilder builder) {
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Config).Assembly));
    }
}