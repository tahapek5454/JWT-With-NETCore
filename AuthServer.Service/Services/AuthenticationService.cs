using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshRespository;

        public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshRespository)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshRespository = userRefreshRespository;
        }


        public async Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if(loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return ResponseDto<TokenDto>.Fail("Email or Password is wrong", true, 400);

            if(! await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return ResponseDto<TokenDto>.Fail("Email or Password is wrong", true, 400);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshRespository.Where(rt => rt.UserId == user.Id).FirstOrDefaultAsync();

            if(userRefreshToken== null)
            {
                await _userRefreshRespository.AddAsync(new UserRefreshToken { UserId = user.Id, Code= token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Sucess(token, 200);
        }

        public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            if(clientLoginDto == null) throw new ArgumentNullException(nameof(clientLoginDto));

            var client = _clients.SingleOrDefault(c => c.Id == clientLoginDto.ClintId && c.Secret == clientLoginDto.ClientSecret);

            if(client == null)
            {
                return ResponseDto<ClientTokenDto>.Fail("ClientId or ClientSecret Not Found", true,404);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return ResponseDto<ClientTokenDto>.Sucess(token, 200);
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existReFreshToken = await _userRefreshRespository.Where(rt => rt.Code.Equals(refreshToken)).FirstOrDefaultAsync();

            if (existReFreshToken == null) return ResponseDto<TokenDto>.Fail("RefreshToken Not Found", true, 404);

            var user = await _userManager.FindByIdAsync(existReFreshToken.UserId.ToString());
            if (user == null) throw new Exception("Data Binding Error Check AuthenditcationService relation userId -> refreshToken");

            var token = _tokenService.CreateToken(user);

            existReFreshToken.Code = token.RefreshToken;
            existReFreshToken.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Sucess(token, 200);
        }

        public async Task<ResponseDto<TokenDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshRespository.Table.Where(rt => rt.Code.Equals(refreshToken)).FirstOrDefaultAsync();
            if(existRefreshToken == null) return ResponseDto<TokenDto>.Fail("RefreshToken Not Found", true, 404);

            _userRefreshRespository.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Sucess(200);
        }
    }
}
