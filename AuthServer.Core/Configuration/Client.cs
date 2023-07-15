using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{
    public class Client
    {
        // oAuth 2.0 
        public string Id { get; set; }
        public string Secret { get; set; }

        // detecting which server the client can access 
        //  no sub for ex www.myapi3.com
        public List<string> Audiences { get; set; }
    }
}
