using System.Threading.Tasks;
using Abp.Application.Services;
using App.Caliset.Sessions.Dto;

namespace App.Caliset.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
