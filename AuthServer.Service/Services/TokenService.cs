using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> options)
        {
            _userManager = userManager;
            _customTokenOptions = options.Value;
     
        }

        public TokenDto CreateToken(UserApp userApp)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }

        private string CreateRefreshToken()
        {
            //32 byte random string data

            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

        }

        private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences) 
        {
            // token's payload is claims

            // if you want you can write "id","1" - "email","example.com" ...
            // but if you want the claims to pair with identity lib you can use const type 
            // For example you can use in controller User.Identity.Name it pair to ClaimTypes.Name
            // otherwise you must User.claims(c => c.type == "myUserName")
            var userClaimList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new Claim(ClaimTypes.Email, userApp.Email),
                new Claim(ClaimTypes.Name, userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // the last one for like a pk
            // this claims about user, after created jwt they added to payload

            userClaimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            // we apply to format to arch can find Audiencess 
        
            return userClaimList;
        }
    }
}
