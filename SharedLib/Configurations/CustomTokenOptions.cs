using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Configurations
{
    public class CustomTokenOptions
    {
        public List<String> Audience { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = default!;
    }
}
