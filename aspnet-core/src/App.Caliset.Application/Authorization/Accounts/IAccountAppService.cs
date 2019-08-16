using System.Threading.Tasks;
using Abp.Application.Services;
using App.Caliset.Authorization.Accounts.Dto;

namespace App.Caliset.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
