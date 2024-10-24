using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dto_s
{
    public class ClientLoginDto
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
