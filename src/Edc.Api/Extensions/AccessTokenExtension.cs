using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Edc.Core;
using Edc.Core.AccountContext.UseCases.Login;
using Microsoft.IdentityModel.Tokens;

namespace Edc.Api.Extensions;

public static class AccessTokenExtension {
    public static string GenerateToken(ResponseData account) {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Config.Secrets.TokenPrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(nameof(account.Id), account.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, account.Name));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, account.Email));
        foreach(var role in account.Roles)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }
}
