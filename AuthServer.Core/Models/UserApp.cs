using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserApp: IdentityUser<int>
    {
        public string City { get; set; }
        public List<Product> Products { get; set; }
        public UserRefreshToken UserRefreshToken { get; set; }

    }
}
