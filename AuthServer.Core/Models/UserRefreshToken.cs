using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public UserApp User { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
