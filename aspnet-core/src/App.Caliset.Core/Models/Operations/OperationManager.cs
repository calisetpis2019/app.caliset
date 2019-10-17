using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
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
                throw new UserFriendlyException("Error", "Ya existe operación.");
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
                throw new UserFriendlyException("Error", "No existe operación.");
            }
            else
            {
                _repositoryOperation.Delete(oper);
            }
        }

        public IEnumerable<Operation> GetAll()
        {
            var aux = _repositoryOperation.GetAll()
               .Include(x => x.OperationType)
               .Include(y => y.OperationState)
               .Include(w => w.Charger)
               .Include(q => q.Nominator)
               .Include(a => a.Location)
                 .Include(z => z.Manager)
                 .Include(asset => asset.Comments)
                 .Include(asset => asset.Samples);
         


            return aux;
        }

        public Operation GetOperationById(int id)
        {
            var oper = _repositoryOperation.Get(id) ;
          
            return oper;
        }

        public void Update(Operation entity)
        {
            _repositoryOperation.Update(entity);
        }

        public IEnumerable<Operation> GetAllFilters(string keyword, int? operationstateId = null, int? operationTypeId = null, int? locationId = null, int? nominatorId = null, int? chargerId = null, int? managerId = null)
        {
            return this.GetAll().WhereIf(operationstateId.HasValue, oper => oper.OperationStateId == operationstateId)
                .WhereIf(operationTypeId.HasValue, oper => oper.OperationTypeId == operationTypeId)
                .WhereIf(locationId.HasValue, oper => oper.LocationId == locationId)
                .WhereIf(nominatorId.HasValue, oper => oper.NominatorId == nominatorId)
                .WhereIf(chargerId.HasValue, oper => oper.ChargerId == chargerId)
                .WhereIf(managerId.HasValue, oper => oper.ManagerId == managerId)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), x => x.Commodity.Contains(keyword) || x.Package.Contains(keyword) || x.ShipName.Contains(keyword) 
                || x.Destiny.Contains(keyword) || x.ClientReference.Contains(keyword) || x.Line.Contains(keyword) || x.BookingNumber.Contains(keyword) 
                || x.OperationType.Name.Contains(keyword) || x.Location.Name.Contains(keyword) || x.Nominator.Name.Contains(keyword) || x.Charger.Name.Contains(keyword) || x.Manager.Name.Contains(keyword));
        }

        public IEnumerable<Operation> GetCurrentOperations()
        {
            return from Oper in this.GetAll()
                        where Oper.OperationStateId != 3
                        select Oper;
        }

        public void ActivateOperationById(int idOperation)
        {
            var operation = this.GetOperationById(idOperation);
            if (operation.OperationStateId != 1)
            {
                throw new UserFriendlyException("Error", "La operación debe estar en estado Futura");
            }
            operation.OperationStateId = 2;
            this.Update(operation);
        }

        public void ActvateOperations()
        {
            var operations = this.GetAllFilters("", 1).Where(oper => oper.Date <= DateTime.Now);
            foreach (Operation oper in operations)
            {
                this.ActivateOperationById(oper.Id);
                //Envio de notificacion
            }
        }

    }
}
