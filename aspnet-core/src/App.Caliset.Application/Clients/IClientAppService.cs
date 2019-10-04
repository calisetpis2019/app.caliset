using Abp.Application.Services;
using App.Caliset.Clients.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Clients
{
    public interface IClientAppService:IApplicationService
    {
        IEnumerable<GetClientOutput> GetAll();
        Task Create(CreateClientInput input);
        void Update(UpdateClientInput input);
        void Delete(DeleteClientInput input);
        GetClientOutput GetClientById(GetClientInput input);
    
    }
}
