using Appointly.Application.DTOs.RoleChange;
using Appointly.Domain.Entities;
using Mapster;

namespace Appointly.Application.Common.Mapping
{
    public class RoleChangeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RoleChangeRequest, RoleChangeResponseDto>()
                .Map(dest => dest.UserFullName, src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : null)
                .Map(dest => dest.ReviewedByAdminFullName, src => src.ReviewedByAdmin != null ? $"{src.ReviewedByAdmin.FirstName} {src.ReviewedByAdmin.LastName}" : null);
        }
    }
}
