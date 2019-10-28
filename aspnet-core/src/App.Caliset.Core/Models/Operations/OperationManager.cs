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

        public  Operation GetOperationById(int id)
        {
            var oper = _repositoryOperation.Get(id);
            oper.Charger = _repositoryClient.FirstOrDefault(oper.ChargerId);
            oper.Location = _repositoryLocation.FirstOrDefault(oper.LocationId);
            //oper.Manager = await _userManager.(oper.ManagerId);
            oper.Nominator = _repositoryClient.FirstOrDefault(oper.NominatorId);
            oper.OperationState = _repositoryOperationState.FirstOrDefault(oper.OperationStateId);
            oper.OperationType = _repositoryOperationType.FirstOrDefault(oper.OperationTypeId);
            oper.Samples = _sampleManager.GetSamplesByOperation(id).ToList();



            return oper;
        }

        public void Update(Operation entity)
        {
            if (entity.Date > System.DateTime.Now && entity.OperationStateId != 3)
                entity.OperationStateId = 1;
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
                .WhereIf(!keyword.IsNullOrWhiteSpace(), x => (x.Commodity != null && x.Commodity.Contains(keyword)) || (x.Package != null && x.Package.Contains(keyword))
                                                     || (x.ShipName != null && x.ShipName.Contains(keyword)) || (x.Destiny != null && x.Destiny.Contains(keyword)) 
                                                     || (x.ClientReference != null && x.ClientReference.Contains(keyword)) || (x.Line != null && x.Destiny.Contains(keyword)) 
                                                     || (x.BookingNumber != null && x.Destiny.Contains(keyword)) || (x.Notes != null && x.Notes.Contains(keyword))
                                                     || x.OperationType.Name.Contains(keyword) || x.Manager.Name.Contains(keyword) || x.Nominator.Name.Contains(keyword)
                                                     || x.Charger.Name.Contains(keyword) || x.Location.Name.Contains(keyword) || x.OperationState.Name.Contains(keyword)

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
            var userManager = new UserIdentifier(1, manager);
            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData("Se ha activado una operación de la cual es responsable."),
                severity: NotificationSeverity.Info,
                userIds: new[] { userManager }
            );
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
