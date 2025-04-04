using App.Application.Features.User;
using App.Application.Features.User.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    public class UserController(IUserService userService) : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateuserRequest request)
        {
            return CreateActionResult(await userService.CreateUserAsync(request));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return CreateActionResult(await userService.GetUserByUsernameAsync(HttpContext.User.Identity!.Name!));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRoles(string username)
        {
            return CreateActionResult(await userService.CreateUserRolesAsync(username));
        }

    }
}
