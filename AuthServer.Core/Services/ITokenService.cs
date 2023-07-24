using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(UserApp userApp);

        // this just for selected client doesn't exist identity
        ClientTokenDto CreateTokenByClient(Client client);

    }
}
