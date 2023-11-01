using DevInsightForge.Application.Authentication.Commands.AuthenticateUser;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsightForge.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly ISender _sender;
    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(nameof(Authenticate))]
    [AllowAnonymous]
    public async Task<TokenResponseModel> Authenticate(AuthenticateUserCommand command)
    {
        return await _sender.Send(command);
    }
}
