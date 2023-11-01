using MediatR;
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
}
