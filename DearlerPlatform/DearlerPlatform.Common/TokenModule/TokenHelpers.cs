using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DearlerPlatform.Common.TokenModule.Models;
using Microsoft.IdentityModel.Tokens;

namespace DearlerPlatform.Common.TokenModule
{
    public static class TokenHelpers
    {
        public static string CreateToken(JwtTokenModel jwtTokenModel)
        {
            var claims = new []{
                new Claim("Id",jwtTokenModel.Id.ToString()),
                new Claim("CustomerNo",jwtTokenModel.CustomerNo),
                new Claim("CustomerName",jwtTokenModel.CustomerName)
            };
            //生成秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.Security));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:jwtTokenModel.Issuer,
                audience:jwtTokenModel.Audience,
                expires:DateTime.Now.AddMinutes(jwtTokenModel.Expires),
                signingCredentials:creds,
                claims:claims
            );
            var accessToekn = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToekn;
        }
    }
}