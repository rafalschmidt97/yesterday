using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Accounts;
using Api.Core.AutomaticDI;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Core.Security
{
  public class TokenUtilities : ISingleton
  {
    private readonly IConfiguration configuration;

    public TokenUtilities(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public string Generate(Account account)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Sid, account.Id.ToString()),
        new Claim(ClaimTypes.Name, account.Username)
      };

      claims.AddRange(account.AccountRoles.Select(role => 
        new Claim(ClaimTypes.Role, role.Role.Name)
      ));

      var token = new JwtSecurityToken(
        configuration["Jwt:Issuer"],
        configuration["Jwt:Issuer"],
        claims.ToArray(),
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}