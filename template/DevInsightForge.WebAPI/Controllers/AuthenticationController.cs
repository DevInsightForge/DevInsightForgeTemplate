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
public class AuthenticationController : ControllerBase
{

    private readonly ISender _sender;
    public AuthenticationController(ISender sender) => _sender = sender;

    [HttpGet(nameof(GetTokenUser))]
    public async Task<UserResponseModel> GetTokenUser()
    {
        return await _sender.Send(new GetTokenUserQuery());
    }

    [AllowAnonymous]
    [HttpPost(nameof(Register))]
    public async Task<TokenResponseModel> Register(RegisterUserCommand command)
    {
        return await _sender.Send(command);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Authenticate))]
    public async Task<TokenResponseModel> Authenticate(AuthenticateUserCommand command)
    {
        return await _sender.Send(command);
    }
}
