using AuthServer.Core.Dto_s;
using SharedLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshAsync(string refreshToken);
        Task<Response<NoDataDto>> RevokeRefreshAsync(string refreshToken); //Logout - Token Steal Scenarios
        Response<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto clientLoginDto);

    }
}
