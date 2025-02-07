using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config=config;
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser user)

        {
    Console.WriteLine($"DEBUG: Creating token for user.UserName = '{user.UserName}'");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email
                ),
                new Claim(JwtRegisteredClaimNames.GivenName,user.UserName)
            };


            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials=creds,
                Issuer=_config["JWT:Issuer"],
                Audience= _config["JWT:Audience"]

            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
//
var tokenString = tokenHandler.WriteToken(token);

// Decode the token to inspect the payload:
var tokenObj = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
var claimsInToken = tokenObj.Claims.Select(c => $"{c.Type} = {c.Value}");
Console.WriteLine("DEBUG: Token claims:");
foreach (var c in claimsInToken)
{
    Console.WriteLine($"    {c}");
}
            return tokenHandler.WriteToken(token);

        }
    }
}