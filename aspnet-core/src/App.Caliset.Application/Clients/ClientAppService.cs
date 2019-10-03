using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using App.Caliset.Clients.Dto;
using App.Caliset.Models.Clients;

namespace App.Caliset.Clients
{
    public class ClientAppService : ApplicationService, IClientAppService
    {
        private readonly IClientManager _clientManager;
        public ClientAppService(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }
        public async Task Create(CreateClientInput input)
        {
            var client = ObjectMapper.Map<Client>(input);
            await _clientManager.Create(client);

        }

        public void Delete(DeleteClientInput input)
        {
            _clientManager.Delete(input.Id);

        }

        public IEnumerable<GetClientOutput> GetAll()
        {
            var getAll = _clientManager.GetAll().ToList();
            List<GetClientOutput> output = ObjectMapper.Map<List<GetClientOutput>>(getAll);
            return output;
        }


        public GetClientOutput GetClientById(GetClientInput input)
        {
            var getClient = _clientManager.GetClientById(input.Id);
            GetClientOutput output = ObjectMapper.Map<GetClientOutput>(getClient);
            return output;
        }

        public void Update(UpdateClientInput input)
        {
            var client = _clientManager.GetClientById(input.Id);
            ObjectMapper.Map(input, client);
            _clientManager.Update(client);
        }
    }
}
