using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.ViewModels.User;

namespace DevInsightForge.Application.Authentication.Queries.GetTokenUser;

public sealed record GetTokenUserQuery : IRequest<UserResponseModel>;

internal sealed class GetTokenUserQueryHandler : IRequestHandler<GetTokenUserQuery, UserResponseModel>
{
    private readonly TokenServices _tokenServices;

    public GetTokenUserQueryHandler(TokenServices tokenServices)
    {
        _tokenServices = tokenServices;
    }

    public Task<UserResponseModel> Handle(GetTokenUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_tokenServices.GetLoggedInUser());
    }
}
