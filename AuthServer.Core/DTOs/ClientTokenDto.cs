using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.DTOs
{
    public class ClientTokenDto
    {
        // just access token for selected client not for subscribe
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
