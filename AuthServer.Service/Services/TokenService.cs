using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
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

        public async Task<TokenDto> CreateToken(UserApp userApp)
        {
            // perapare token option from configuration
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

            // get signging algorithm for token with our keys
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create token operations
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: await GetClaims(userApp, _customTokenOptions.Audiences));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            // we convert to custom dto for open the outside
            var tokenDto = new TokenDto()
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.RefreshTokenExpiration)
            };

            return tokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            // perapare token option from configuration
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

            // get signging algorithm for token with our keys
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create token operations
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: GetClaimsByClient(client));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            // we convert to custom dto for open the outside
            var clientTokenDto = new ClientTokenDto()
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
            };

            // doesnt exist refresh token
            return clientTokenDto;

        }

        private string CreateRefreshToken()
        {
            //32 byte random string data

            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

        }

        private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audiences) 
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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("city", userApp.City) // we dont have const arch type so we wrote manuel
            };
            // the last one for like a pk
            // this claims about user, after created jwt they added to payload

            userClaimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            // we apply to format to arch can find Audiencess 


            // now we get user roles and add to claims
            var userRoles = await _userManager.GetRolesAsync(userApp);
            userClaimList.AddRange(userRoles.Select(r => new Claim(ClaimTypes.Role, r)));
            
        
            return userClaimList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            // this for clients
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, client.Id), // subject for who ? client

            };
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            // which api client can request

            return claims;
        }
    }
}
