using DevInsightForge.Application.Authentication.Commands.AuthenticateUser;
using DevInsightForge.Application.Authentication.Commands.RegisterUser;
using DevInsightForge.Application.Authentication.Queries.GetTokenUser;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using DevInsightForge.Application.Common.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsightForge.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpGet(nameof(GetTokenUser))]
    public async Task<UserResponseModel> GetTokenUser()
    {
        return await sender.Send(new GetTokenUserQuery());
    }

    [AllowAnonymous]
    [HttpPost(nameof(RegisterUser))]
    public async Task<TokenResponseModel> RegisterUser(RegisterUserCommand command)
    {
        return await sender.Send(command);
    }

    [AllowAnonymous]
    [HttpPost(nameof(AuthenticateUser))]
    public async Task<TokenResponseModel> AuthenticateUser(AuthenticateUserCommand command)
    {
        return await sender.Send(command);
    }
}
