using AuthServer.Core.Configuration;
using AuthServer.Core.Dto_s;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLib.Dtos;
using System.Net;

namespace AuthServer.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService
            , UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }
        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) { return Response<TokenDto>.Fail("Email or Password Is Wrong", 400, true); }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) { return Response<TokenDto>.Fail("Email or Password Is Wrong", 400, true); }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                //Update Refresh Token
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, (int)HttpStatusCode.OK);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("ClientId or ClientSecret Not Found", (int)HttpStatusCode.NotFound, true);

            }

            var token = _tokenService.CreateClientToken(client);
            return Response<ClientTokenDto>.Success(token, (int)HttpStatusCode.OK);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshAsync(string refreshToken)
        {
            var existsRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existsRefreshToken == null) { return Response<TokenDto>.Fail("RefreshToken Not Found", (int)HttpStatusCode.NotFound, true); }

            var user = await _userManager.FindByIdAsync(existsRefreshToken.UserId);

            if (user == null)
            {
                return Response<TokenDto>.Fail("User Not Found", (int)HttpStatusCode.NotFound, true);
            }

            var token = _tokenService.CreateToken(user);

            existsRefreshToken.Code = token.RefreshToken;
            existsRefreshToken.Expiration = token.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, (int)HttpStatusCode.OK);
        }
        public async Task<Response<NoDataDto>> RevokeRefreshAsync(string refreshToken)
        {
            var existsRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();


            if (existsRefreshToken == null) { return Response<NoDataDto>.Fail("RefreshToken Not Found", (int)HttpStatusCode.NotFound, true); }

            _userRefreshTokenService.Delete(existsRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success((int)HttpStatusCode.NoContent);

        }
    }
}
