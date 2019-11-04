using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Notifications;
using Abp.UI;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Clients;
using App.Caliset.Models.Locations;
using App.Caliset.Models.Notifications;
using App.Caliset.Models.OperationStates;
using App.Caliset.Models.OperationTypes;
using App.Caliset.Models.Samples;
using App.Caliset.Models.UserDeviceTokens;
using Microsoft.EntityFrameworkCore;

namespace App.Caliset.Models.Operations
{
    public class OperationManager : DomainService, IOperationManager
    {
        private readonly IRepository<Operation> _repositoryOperation;
        private readonly UserDeviceTokenManager _userDeviceTokenManager;
        private readonly NotificationManager _notificationManager;
        private readonly NotificationPublisher _notificationPublisher;
        private readonly UserManager _userManager;
        private readonly IRepository<Assignation> _repositoryAssignation;
   
        private readonly IRepository<Location> _repositoryLocation;
        private readonly IRepository<OperationType> _repositoryOperationType;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<OperationState> _repositoryOperationState;
        private readonly SampleManager _sampleManager;



        public OperationManager(IRepository<Operation> repositoryOperation, IRepository<Assignation> repositoryAssignation, UserDeviceTokenManager userDeviceTokenManager,
                                NotificationManager notificationManager, NotificationPublisher notificationPublisher, UserManager userManager, 
                                IRepository<Location> repositoryLocation , IRepository<OperationType> repositoryOperationType,
                                IRepository<Client> repositoryClient, IRepository<OperationState> repositoryOperationState,
                                SampleManager sampleManager)
        {
            _repositoryOperation = repositoryOperation;
            _repositoryAssignation = repositoryAssignation;
            _userDeviceTokenManager = userDeviceTokenManager;
            _notificationManager = notificationManager;
            _notificationPublisher = notificationPublisher;
            _userManager = userManager;
            _repositoryLocation = repositoryLocation;
            _repositoryOperationType = repositoryOperationType;
            _repositoryClient = repositoryClient;
            _repositoryOperationState = repositoryOperationState;
            _sampleManager = sampleManager;

        }



        public async Task<int> Create(Operation entity)
        {
            var oper = _repositoryOperation.FirstOrDefault(x => x.Id == entity.Id);
            if (oper != null)
            {
                throw new UserFriendlyException("Error", "Ya existe operación.");
            }
            else
            {
                return  await _repositoryOperation.InsertAndGetIdAsync(entity);
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
                 .Include(asset => asset.Samples)
                 .Include(asset => asset.HoursRecord);
         


            return aux;
        }

        public Operation GetOperationById(int id)
        {
            var oper = _repositoryOperation.Get(id);
            oper.Charger = _repositoryClient.FirstOrDefault(oper.ChargerId);
            oper.Location = _repositoryLocation.FirstOrDefault(oper.LocationId);
            oper.Manager =  _userManager.FindByIdAsync(oper.ManagerId.ToString()).Result;
            oper.Nominator = _repositoryClient.FirstOrDefault(oper.NominatorId);
            oper.OperationState = _repositoryOperationState.FirstOrDefault(oper.OperationStateId);
            oper.OperationType = _repositoryOperationType.FirstOrDefault(oper.OperationTypeId);
            oper.Samples = _sampleManager.GetSamplesByOperation(id).ToList();



            return oper;
        }

        public void Update(Operation entity)
        {
            if ((entity.Date > System.DateTime.Now) && entity.OperationStateId != 3)
                entity.OperationStateId = 1;
            else if((entity.Date <= System.DateTime.Now) && entity.OperationStateId != 3)
                entity.OperationStateId = 2;

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
                .WhereIf(!keyword.IsNullOrWhiteSpace(), x => (x.Commodity != null && x.Commodity.ToUpper().Contains(keyword.ToUpper())) || (x.Package != null && x.Package.ToUpper().Contains(keyword.ToUpper()))
                                                     || (x.ShipName != null && x.ShipName.ToUpper().Contains(keyword.ToUpper())) || (x.Destiny != null && x.Destiny.ToUpper().Contains(keyword.ToUpper())) 
                                                     || (x.ClientReference != null && x.ClientReference.ToUpper().Contains(keyword.ToUpper())) || (x.Line != null && x.Line.ToUpper().Contains(keyword.ToUpper())) 
                                                     || (x.BookingNumber != null && x.BookingNumber.ToUpper().Contains(keyword.ToUpper())) || (x.Notes != null && x.Notes.ToUpper().Contains(keyword.ToUpper()))
                                                     || x.OperationType.Name.ToUpper().Contains(keyword.ToUpper()) || x.Manager.Name.ToUpper().Contains(keyword.ToUpper()) || x.Nominator.Name.ToUpper().Contains(keyword.ToUpper())
                                                     || x.Charger.Name.ToUpper().Contains(keyword.ToUpper()) || x.Location.Name.ToUpper().Contains(keyword.ToUpper()) || x.OperationState.Name.ToUpper().Contains(keyword.ToUpper())

               );
        }

        public IEnumerable<Operation> GetCurrentOperations()
        {
            return from Oper in this.GetAll()
                        where Oper.OperationStateId != 3
                        select Oper;
        }

        public async Task ActivateOperationById(int idOperation)
        {
            var operation = this.GetOperationById(idOperation);
            if (operation.OperationStateId != 1)
            {
                throw new UserFriendlyException("Error", "La operación debe estar en estado Futura");
            }
            operation.OperationStateId = 2;
            operation.OperationState = _repositoryOperationState.FirstOrDefault(x => x.Id == 2);
            var manager = operation.ManagerId;

            this.Update(operation);
            //NOTIFICATIONS MOBILE
            var inspectors = (from Insp in _userManager.GetAll()
                              join Assign in _repositoryAssignation.GetAll().Where(a => a.OperationId == idOperation && a.Aware == true).ToList()
                              on Insp.Id equals Assign.InspectorId
                              select Insp.Id).Distinct();

            if (inspectors.Count() > 0)
            {
                foreach (int insp in inspectors)
                {
                    if (_userDeviceTokenManager.getById(insp) != null)
                    {
                        _notificationManager.sendNotification("Operacion Activa", "Se ha activado una operacion a la que está asignado.", insp);
                    }
                }

            }

            //NOTIFICATIONS FW
            //var userManager = new UserIdentifier(1, manager);
            //await _notificationPublisher.PublishAsync(
            //    "App.SimpleMessage",
            //    new MessageNotificationData("Se ha activado una operación de la cual es responsable."),
            //    severity: NotificationSeverity.Info,
            //    userIds: new[] { userManager }
            //);

            // NOTIFICACION A RESPONSABLE
            if (_userDeviceTokenManager.getById(manager) != null)
                _notificationManager.sendNotification("Operacion Activa", "Se ha activado una operacion a la que está asignado.", manager);
        }

        public async Task ActvateOperations()
        {
            var operations = this.GetAll().Where(oper => oper.OperationStateId == 1).Where(oper => oper.Date <= DateTime.Now.AddHours(6));

            foreach (Operation oper in operations)
            {
                await this.ActivateOperationById(oper.Id);
            }
        }

    }
}
