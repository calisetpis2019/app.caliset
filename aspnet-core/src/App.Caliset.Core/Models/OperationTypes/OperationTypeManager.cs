using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.Models.OperationTypes
{
    public class OperationTypeManager : DomainService, IOperationTypeManager
    {
        private readonly IRepository<OperationType> _repositoryOperationType;
        public OperationTypeManager(IRepository<OperationType> repositoryOperationType)
        {
            _repositoryOperationType = repositoryOperationType;
        }

        public async Task<OperationType> Create(OperationType entity)
        {
            var operationType = _repositoryOperationType.GetAll().Where(x => x.Id == entity.Id || x.Name == entity.Name);
            if (operationType.Count() > 0)
            {
                throw new UserFriendlyException("Ya existe tipo de operación.");
            }
            else
            {
                return await _repositoryOperationType.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var operationType = _repositoryOperationType.FirstOrDefault(x => x.Id == id);
            if (operationType == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryOperationType.Delete(operationType);
            }
        }

        public IEnumerable<OperationType> GetAll()
        {
            return _repositoryOperationType.GetAll();
        }


        public OperationType GetOperationTypeById(int id)
        {
            return _repositoryOperationType.Get(id);
        }

        public void Update(OperationType entity)
        {
            _repositoryOperationType.Update(entity);
        }
    }
}
