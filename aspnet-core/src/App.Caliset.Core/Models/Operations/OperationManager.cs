using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;

namespace App.Caliset.Models.Operations
{
    public class OperationManager : DomainService, IOperationManager
    {


        private readonly IRepository<Operation> _repositoryOperation;

        public OperationManager(IRepository<Operation> repositoryOperation)
        {
            _repositoryOperation = repositoryOperation;

        }



        public async Task<Operation> Create(Operation entity)
        {
            var oper = _repositoryOperation.FirstOrDefault(x => x.Id == entity.Id);
            if (oper != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                return await _repositoryOperation.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {

            var oper = _repositoryOperation.FirstOrDefault(x => x.Id == id);
            if (oper == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryOperation.Delete(oper);
            }
        }

        public IEnumerable<Operation> GetAll()
        {
            return _repositoryOperation.GetAll()
               .Include(asset=> asset.OperationType)
               .Include(asset => asset.OperationState)
               .Include(asset => asset.Charger)
               .Include(asset => asset.Nominator)
               .Include(asset => asset.Location)
               .Include(asset => asset.Manager)
                //.Include(asset => asset.Comments)
                // .Include(asset => asset.Samples)
                // .Include(asset => asset.Inspectors)
                ;
        }

        public Operation GetOperationById(int id)
        {
            return _repositoryOperation.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Operation entity)
        {
            _repositoryOperation.Update(entity);
        }
    }
}
