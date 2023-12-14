using DTO.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InoviWebApi.Helper
{
    public class JWT
    {
        //private readonly IConfiguration _configuration;

        //public JWT(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        //public string GenerateJWT(LoginDTO req)
        //{

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[] {
        // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        // new Claim(JwtRegisteredClaimNames.Email, req.UserEmail),
        // new Claim(JwtRegisteredClaimNames.Sid, req.UserPassword),
        // new Claim("Date", DateTime.Now.ToString()),
        // };

        //    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
        //      _configuration["Jwt:Audiance"],
        //      claims,
        //      expires: DateTime.Now.AddMinutes(2),
        //      signingCredentials: credentials);
        //    req.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //    return req.Token;
        //}

    }
}
