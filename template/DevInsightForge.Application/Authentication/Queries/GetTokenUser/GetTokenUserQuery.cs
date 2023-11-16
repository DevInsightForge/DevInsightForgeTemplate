using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.ViewModels.User;

namespace DevInsightForge.Application.Authentication.Queries.GetTokenUser;

public sealed record GetTokenUserQuery : IRequest<UserResponseModel>;

internal sealed class GetTokenUserQueryHandler(TokenServices tokenServices) : IRequestHandler<GetTokenUserQuery, UserResponseModel>
{
    public Task<UserResponseModel> Handle(GetTokenUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(tokenServices.GetLoggedInUser());
    }
}
