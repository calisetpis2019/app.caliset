using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Models.OperationStates
{
    public interface IOperationStateManager : IDomainService
    {
        IEnumerable<OperationState> GetAll();
        OperationState GetOperationStateById(int id);
        Task<OperationState> Create(OperationState entity);
        void Update(OperationState entity);
        void Delete(int id);
    }
}
