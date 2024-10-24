using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dto_s
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiration { get; set; }
    }
}
