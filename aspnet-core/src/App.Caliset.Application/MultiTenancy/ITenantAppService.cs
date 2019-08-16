using Abp.Application.Services;
using Abp.Application.Services.Dto;
using App.Caliset.MultiTenancy.Dto;

namespace App.Caliset.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

