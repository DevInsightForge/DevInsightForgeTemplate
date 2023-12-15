using DevInsightForge.Application.Common.ViewModels.User;
using DevInsightForge.Domain.Entities.Core;

namespace DevInsightForge.Application.Common.Configurations.Mapster;

public static class MappingConfigurations
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<UserModel, UserResponseModel>
            .NewConfig()
            .Map(dest => dest.UserId, src => src.Id);
    }
}
