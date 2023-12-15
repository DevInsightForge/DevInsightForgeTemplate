using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.ViewModels.Authentication;

namespace DevInsightForge.Application.Authentication.Queries.GetTokenUser;

public sealed record GetTokenUserQuery : IRequest<TokenUserModel>;

internal sealed class GetTokenUserQueryHandler(TokenServices tokenServices) : IRequestHandler<GetTokenUserQuery, TokenUserModel>
{
    public Task<TokenUserModel> Handle(GetTokenUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(tokenServices.GetLoggedInUser());
    }
}
