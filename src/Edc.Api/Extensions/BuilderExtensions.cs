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

}