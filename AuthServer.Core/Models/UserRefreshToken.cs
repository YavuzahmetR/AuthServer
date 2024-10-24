using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        public string UserId { get; set; } = default!; //which user uses
        public string Code { get; set; } = default!; //token itself
        public DateTime Expiration { get; set; } //lifetime
    }
}
