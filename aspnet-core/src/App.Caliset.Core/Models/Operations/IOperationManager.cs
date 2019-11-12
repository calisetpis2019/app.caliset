using Abp.Domain.Services;
using App.Caliset.Models.Forms;
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
        Task<int> Create(Operation entity);
        void Update(Operation entity);
        void Delete(int id);
        IEnumerable<Operation> GetAllFilters(string keyword, int? operationstateId, int? operationTypeId, int? locationId, int? nominatorId, int? chargerId, int? managerId);
        Task ActivateOperationById(int idOperation);
        Task ActvateOperations();
        void AddForm(FormOperation FO);
        void AntiAddForm(FormOperation FO);
        IEnumerable<Form> GetFormsByOperation(int IdOperation);
        IEnumerable<Form> NoGetFormsByOperation(int IdOperation);
    }
}
