using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;
using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.ViewModels.User;
using DevInsightForge.Domain.Entities.Core;

namespace DevInsightForge.Application.Authentication.Queries.GetTokenUser;

public sealed record GetTokenUserQuery : IRequest<UserResponseModel>;

internal sealed class GetTokenUserQueryHandler(
    IUserRepository userRepository, 
    TokenServices tokenServices) : IRequestHandler<GetTokenUserQuery, UserResponseModel>
{
    public async Task<UserResponseModel> Handle(GetTokenUserQuery request, CancellationToken cancellationToken)
    {
        Guid userId = tokenServices.GetLoggedInUserId();
        UserModel? user = await userRepository.GetWhereAsync(u => u.Id.Equals(userId));
        ArgumentNullException.ThrowIfNull(user, "No user found from the token");

        return user.Adapt<UserResponseModel>();
    }
}
