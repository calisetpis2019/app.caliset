using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.Models.OperationStates
{
    public class OperationStateManager : DomainService, IOperationStateManager
    {
        private readonly IRepository<OperationState> _repositoryOperationState;
        public OperationStateManager(IRepository<OperationState> repositoryOperationState)
        {
            _repositoryOperationState = repositoryOperationState;
        }

        public async Task<OperationState> Create(OperationState entity)
        {
            var operationState = _repositoryOperationState.GetAll().Where(x => x.Id == entity.Id || x.Name == entity.Name);
            if (operationState.Count() > 0)
            {
                throw new UserFriendlyException("Error", "Ya existe estado.");
            }
            else
            {
                return await _repositoryOperationState.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var operationState = _repositoryOperationState.FirstOrDefault(x => x.Id == id);
            if (operationState == null)
            {
                throw new UserFriendlyException("Error", "No existe estado.");
            }
            else
            {
                _repositoryOperationState.Delete(operationState);
            }
        }

        public IEnumerable<OperationState> GetAll()
        {
            return _repositoryOperationState.GetAll();
        }


        public OperationState GetOperationStateById(int id)
        {
            return _repositoryOperationState.Get(id);
        }

        public void Update(OperationState entity)
        {
            _repositoryOperationState.Update(entity);
        }
    }
}
