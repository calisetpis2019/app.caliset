using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var client = _repositoryClient.GetAll().Where(x => x.Id == entity.Id || x.Name == entity.Name);
            if (client != null)
            {
                throw new UserFriendlyException("Ya existe cliente.");
            }
            else
            {                
                try
                {
                    return await _repositoryClient.InsertAsync(entity);
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);

                }
                
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
