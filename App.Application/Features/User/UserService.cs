using App.Application.Features.User.Create;
using App.Application.Features.User.Dto;
using App.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace App.Application.Features.User
{
    public class UserService(
        UserManager<UserApp> userManager,
        IMapper mapper) : IUserService
    {
        public async Task<ServiceResult<UserAppDto>> CreateUserAsync(CreateuserRequest request)
        {
            UserApp user = new()
            {
                Email = request.Email,
                UserName = request.Username,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return ServiceResult<UserAppDto>.Fail(errors);
            }
            
            var userAppDto = mapper.Map<UserAppDto>(user);

            return ServiceResult<UserAppDto>.Success(userAppDto);
        }

        public async Task<ServiceResult<UserAppDto>> GetUserByUsernameAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if(user == null)
                return ServiceResult<UserAppDto>.Fail("Username not found", System.Net.HttpStatusCode.NotFound);

            var userAppDto = mapper.Map<UserAppDto>(user);
            return ServiceResult<UserAppDto>.Success(userAppDto);
        }
    }
}
