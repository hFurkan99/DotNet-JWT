using App.Application.Features.User.Create;
using App.Application.Features.User.Dto;

namespace App.Application.Features.User
{
    public interface IUserService
    {
        Task<ServiceResult<UserAppDto>> CreateUserAsync(CreateuserRequest request);
        Task<ServiceResult<UserAppDto>> GetUserByUsernameAsync(string username);
    }
}
