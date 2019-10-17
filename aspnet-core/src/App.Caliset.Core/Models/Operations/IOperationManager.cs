using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Operations
{
    public interface IOperationManager: IDomainService
    {
        IEnumerable<Operation> GetAll();
        Operation GetOperationById(int id);
        Task<Operation> Create(Operation entity);
        void Update(Operation entity);
        void Delete(int id);
        IEnumerable<Operation> GetAllFilters(string keyword, int? operationstateId, int? operationTypeId, int? locationId, int? nominatorId, int? chargerId, int? managerId);
        void ActivateOperationById(int idOperation);
        void ActvateOperations();
    }
}
