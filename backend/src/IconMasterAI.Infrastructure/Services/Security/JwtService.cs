using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Services.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace IconMasterAI.Infrastructure.Services.Security;

internal sealed class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public JwtService(IConfiguration configuration)
    {
        var keyBytes = Encoding.UTF8.GetBytes(
            configuration["Jwt:Secret"] ?? throw new Exception("No JWT Secret in configuration."));

        _configuration = configuration;
        _key = new SymmetricSecurityKey(keyBytes);
    }

    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpiresInDays")),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

        var tokenStr = handler.WriteToken(token);
        return tokenStr;
    }
}
