using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Models.OperationTypes
{
    public interface IOperationTypeManager : IDomainService
    {
        IEnumerable<OperationType> GetAll();
        OperationType GetOperationTypeById(int id);
        Task<OperationType> Create(OperationType entity);
        void Update(OperationType entity);
        void Delete(int id);
    }
}
