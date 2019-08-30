using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SenseHMS.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SenseHMS.Controllers
{
    public class TokenController : Controller 
    {
        private const string SECRET_KEY = "TQvgjeABMPOwCycOqah5EQu5yyVjpmVG";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenController.SECRET_KEY));

        //[HttpGet]
        //[Route("api/token/{username}/{password}")]
        //public IActionResult Get(string username, string password)
        //{
        //    if (username == password)
        //    {
        //        return new ObjectResult(GenarateToken(username));
        //    }
        //    else
        //        return BadRequest();
        //}

        //private string GenarateToken(string username)
        //{
        //    var token = new JwtSecurityToken(
        //        claims: new Claim[] { new Claim(ClaimTypes.Name, username) },
        //        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
        //        expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
        //        signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        //public async Task<IActionResult> Login(LoginModel model)
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Post([FromBody]LoginModel model)
        {
            if (model.username == model.password)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", model.username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var sequrityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(sequrityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}
