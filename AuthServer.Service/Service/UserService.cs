using AuthServer.Core.Dto_s;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLib.Dtos;
using System.Net;

namespace AuthServer.Service.Service
{
    public class UserService(UserManager<UserApp> _userManager) : IUserService
    {
        
        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { Email = createUserDto.Email , UserName = createUserDto.UserName};

           //user property passwordHash will be filled after the method executes below.
            var result = await _userManager.CreateAsync(user,createUserDto.Password);


            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors,true), (int)HttpStatusCode.BadRequest);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),(int)HttpStatusCode.OK);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return Response<UserAppDto>.Fail("User Not Found", (int)HttpStatusCode.NotFound,true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), (int)HttpStatusCode.OK);
        }
    }
}
