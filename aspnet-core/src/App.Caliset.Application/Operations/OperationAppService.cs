using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Notifications;
using Abp.UI;
using App.Caliset.Assignations.Dto;
using App.Caliset.Authorization;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Clients;
using App.Caliset.Models.Locations;
using App.Caliset.Models.Notifications;
using App.Caliset.Models.Operations;
using App.Caliset.Models.OperationStates;
using App.Caliset.Models.OperationTypes;
using App.Caliset.Models.UserDeviceTokens;
using App.Caliset.Operations.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Operations
{
    public class OperationAppService : ApplicationService, IOperationAppService
    {


        private readonly IOperationManager _operationManager;
        private readonly AssignationManager _assignationManager;
        private readonly NotificationManager _notificationManager;
        private readonly UserDeviceTokenManager _userDeviceTokerManager;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IUserNotificationManager _notificationManagerFW;
        private readonly ILocationManager _locationManager;
        private readonly IOperationTypeManager _operationTypeManager;
        private readonly IClientManager _clientManager;
        private readonly UserManager _userManager;



        public OperationAppService(IOperationManager operationManager
                                    , AssignationManager assignationManager
                                    , NotificationManager notificationManager
                                    , UserDeviceTokenManager userDeviceTokerManager
                                    , INotificationPublisher notificationPublisher
                                    , IUserNotificationManager notificationManagerFW
                                    , ILocationManager locationManager
                                    , IOperationTypeManager operationTypeManager
                                    , IClientManager clientManager
                                    , UserManager userManager

            )
        {
            _operationManager = operationManager;
            _assignationManager = assignationManager;
            _notificationManager = notificationManager;
            _userDeviceTokerManager = userDeviceTokerManager;
            _notificationPublisher = notificationPublisher;
            _notificationManagerFW = notificationManagerFW;
            _locationManager = locationManager;
            _operationTypeManager = operationTypeManager;
            _clientManager = clientManager;
            _userManager = userManager;
        }


        [AbpAuthorize(PermissionNames.Operador)]
        public async Task Create(CreateOperationInput input)
        {
            var operation = ObjectMapper.Map<Operation>(input);
            if (input.Date < DateTime.Today)
                operation.OperationStateId = 2;
            else
                operation.OperationStateId = 1;


            var op =  _operationManager.Create(operation).Result;

            //var message = "Ha sido asignado como responsable de una operación.";

            //var userManager = new UserIdentifier(1, input.ManagerId);

            //await _notificationPublisher.PublishAsync(
            //    "App.SimpleMessage",
            //    new MessageNotificationData(message),
            //    severity: NotificationSeverity.Info,
            //    userIds: new[] { userManager }
            //);

            if(_userDeviceTokerManager.getById(op.ManagerId) != null)
                _notificationManager.sendNotification("Operacion nueva", "Se le asigno como responsable en la operacion #" ,op.Id);
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public async Task Delete(DeleteOperationInput input)
        {
            var operation = _operationManager.GetOperationById(input.Id);
            if (operation.OperationStateId == 3)
            {
                throw new UserFriendlyException("Error", "Operación finalizada, no se puede eliminar.");
            } else
            {
                _operationManager.Delete(input.Id);
            }

        }

        [AbpAuthorize(PermissionNames.Operador)]
        public void EndOperation(GetOperationInput input)
        {
            var operation = _operationManager.GetOperationById(input.Id);
            if (operation.OperationStateId == 3)
            {
                throw new UserFriendlyException("Error", "Operación finalizada, no se puede modificar.");
            }
            operation.OperationStateId = 3;
            _operationManager.Update(operation);
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetAll()
        {
            var getAll = _operationManager.GetAll().ToList();
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(getAll);

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetAllFilters(GetOperationFiltersInput input)
        {
            var operations = _operationManager.GetAllFilters(input.Keyword, input.OperationStateId, input.OperationTypeId, input.LocationId, input.NominatorId, input.ChargerId, input.ManagerId);
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(operations);

            return output;
        }

        public GetOperationOutput GetOperationById(GetOperationInput input)
        {
            var Oper = _operationManager.GetOperationById(input.Id);
            var ret = ObjectMapper.Map<GetOperationOutput>(Oper);

            return ret;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public async Task Update(UpdateOperationInput input)
        {
            var operation = _operationManager.GetOperationById(input.Id);

            String Cambios = "";

            if (input.Date != operation.Date)
                Cambios += " - Fecha: " + input.Date.ToString();
            if (input.Commodity != operation.Commodity)
                Cambios += " - Mercaderia: " + input.Commodity;
            if (input.Package != operation.Package)
                Cambios += " - Empaque: " + input.Package;
            if (input.ShipName != operation.ShipName)
                Cambios += " - Nombre del Barco: " + input.ShipName;
            if (input.Destiny != operation.Destiny)
                Cambios += " - Destino: " + input.Destiny;
            if (input.ClientReference != operation.ClientReference)
                Cambios += " - Referencia Cliente: " + input.ClientReference;
            if (input.Line != operation.Line)
                Cambios += " - Linea: " + input.Line;
            if (input.BookingNumber != operation.BookingNumber)
                Cambios += " - Numero de Booking: " + input.BookingNumber;
            if (input.Notes != operation.Notes)
                Cambios += " -  Notas: " + input.Notes;
            if (input.LocationId != operation.LocationId)
                Cambios += " - Lugar: " + _locationManager.GetLocationById(input.LocationId).Name ;
            if (input.OperationTypeId != operation.OperationTypeId)
                Cambios += " - Tipo: " +_operationTypeManager.GetOperationTypeById(input.OperationTypeId).Name ;
            if (input.NominatorId != operation.NominatorId)
                Cambios += " - Nominador: " + _clientManager.GetClientById(input.NominatorId).Name ;
            if (input.ChargerId != operation.ChargerId)
                Cambios += " - Cargador: " + _clientManager.GetClientById(input.ChargerId).Name;
            if (input.ManagerId != operation.ManagerId)
                Cambios += " - Responsable: " + _userManager.FindByIdAsync(input.ManagerId.ToString()).Result.Name;


            ObjectMapper.Map(input, operation);
            _operationManager.Update(operation);

            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(null,operation.Id));

            List<long> userNotify = new List<long>();

            //ARRANCA NOTIFICACION DE MODIFICACION DE OPERACION-------------------------------------------------------
            foreach (var x in output) 
            {
                if( (x.Aware==true || x.Aware == null) && (_userDeviceTokerManager.getById(x.InspectorId)!= null)) {
                    if (!userNotify.Contains(x.InspectorId)) { 
                        _notificationManager.sendNotification("Operacion Modificada", "Se ha modificado la operacion #"+ input.Id +" a la que esta asignado de la siguiente forma: " + Cambios, x.InspectorId);
                        userNotify.Add(x.InspectorId);
                    }

                }
            }//TERMINA NOTIFICACION DE MODIFICACION DE OPERACION-------------------------------------------------------

            ////NOTIFICATION FW

            //var userManager = new UserIdentifier(1, operation.ManagerId);
            //await _notificationPublisher.PublishAsync(
            //    "App.SimpleMessage",
            //    new MessageNotificationData("Se le ha modificado una operación de la cual es responsable los siguientes cambios: " + Cambios ),
            //    severity: NotificationSeverity.Info,
            //    userIds: new[] { userManager }
            //);

            // ARRANCA NOTIFICACION DE RESPONSABLE

            if(_userDeviceTokerManager.getById(input.ManagerId) != null)
                _notificationManager.sendNotification("Operacion Modificada", "Se ha modificado la operacion #" + input.Id + " en la que es Responsable de la siguiente forma: " + Cambios, input.ManagerId);

        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void UpdateFinishedOperation(UpdateOperationInput input)
        {
            var operation = _operationManager.GetOperationById(input.Id);
            if (operation.OperationStateId != 3)
            {
                throw new UserFriendlyException("Error", "Esta operacion no está finalizada");
            }
            ObjectMapper.Map(input, operation);
            _operationManager.Update(operation);

        }


        [AbpAuthorize(PermissionNames.Operador)]
        public async Task ActivateOperationById(int id)
        {
            await _operationManager.ActivateOperationById(id);
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public async Task ActvateOperations()
        {
            await _operationManager.ActvateOperations();
        }

        public Task<List<UserNotification>> PRUEBA_getNotificationFWByUser(long userId)
        {
            return _notificationManagerFW.GetUserNotificationsAsync(new UserIdentifier(1, userId));
        }

    }
}
