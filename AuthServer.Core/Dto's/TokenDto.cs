using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dto_s
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
