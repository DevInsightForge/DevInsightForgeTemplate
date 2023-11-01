using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;
using DevInsightForge.Domain.Entities.User;
using DevInsightForge.Infrastructure.Persistence;

namespace DevInsightForge.Infrastructure.DataAccess.Repositories;

public class UserRepository : GenericRepository<UserModel>, IUserRepository
{
    public UserRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}
