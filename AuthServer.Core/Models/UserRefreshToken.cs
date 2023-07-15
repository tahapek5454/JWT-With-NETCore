using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
