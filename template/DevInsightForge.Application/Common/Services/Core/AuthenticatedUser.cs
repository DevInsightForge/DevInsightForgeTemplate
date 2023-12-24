using DevInsightForge.Application.Common.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevInsightForge.WebAPI.Services;

public class AuthenticatedUser(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUser
{
    public Guid? UserId
    {
        get
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sid);

            if (userIdString != null && Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return userIdGuid;
            }

            return null;
        }
    }
}
