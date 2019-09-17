using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System.Collections.Generic;
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
            var operationState = _repositoryOperationState.FirstOrDefault(x => x.Id == entity.Id);
            if (operationState != null)
            {
                throw new UserFriendlyException("Already Exist");
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
                throw new UserFriendlyException("No Data Found");
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
