using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public static class SignService
    {
        // asimetrik key for token
        // auth server sign token with private key and api resolve token with public key
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}
