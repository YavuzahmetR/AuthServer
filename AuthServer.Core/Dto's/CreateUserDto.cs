using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dto_s
{
    public class CreateUserDto
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
