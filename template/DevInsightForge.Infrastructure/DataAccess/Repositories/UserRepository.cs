using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;
using DevInsightForge.Domain.Entities.Core;
using DevInsightForge.Infrastructure.Persistence;

namespace DevInsightForge.Infrastructure.DataAccess.Repositories;

public class UserRepository(DatabaseContext dbContext) : GenericRepository<UserModel>(dbContext), IUserRepository
{
}
