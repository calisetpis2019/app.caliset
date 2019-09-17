using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Clients
{
    public class ClientManager : DomainService, IClientManager
    {
        private readonly IRepository<Client> _repositoryClient;

        public ClientManager(IRepository<Client> repositoryClient)
        {
            _repositoryClient = repositoryClient;

        }

        public async Task<Client> Create(Client entity)
        {
            var client = _repositoryClient.FirstOrDefault(x => x.Id == entity.Id);
            if (client != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                return await _repositoryClient.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var client = _repositoryClient.FirstOrDefault(x => x.Id == id);
            if (client == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryClient.Delete(client);
            }
        }

        public IEnumerable<Client> GetAll()
        {
            return _repositoryClient.GetAll();
        }

        public Client GetClientById(int id)
        {
            return _repositoryClient.Get(id);
        }

        public void Update(Client entity)
        {
            _repositoryClient.Update(entity);
        }
    }
}
