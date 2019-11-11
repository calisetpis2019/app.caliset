using Abp.Application.Services;
using App.Caliset.Models.Assignations;
using App.Caliset.Assignations.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Authorization;
using App.Caliset.Authorization;
using App.Caliset.Operations.Dto;

using Abp.Collections.Extensions;
using App.Caliset.Users.Dto;
using App.Caliset.Models.Notifications;
using Abp.Domain.Uow;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.UserDeviceTokens;
using App.Caliset.Mails;

namespace App.Caliset.Assignations
{
    public class AssignationAppService : ApplicationService, IAssignationAppService
    {
        private readonly AssignationManager _assignationManager;
        private readonly IAbpSession _abpSession;
        private readonly NotificationManager _notificationManager;

        public AssignationAppService(AssignationManager assignationManager, IAbpSession abpSession, NotificationManager notificationManager)
        {
            _assignationManager = assignationManager;
            _abpSession = abpSession;
            _notificationManager = notificationManager;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public async Task<bool> Create(CreateAssignationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var assignation = ObjectMapper.Map<Assignation>(input);
            bool resp = await _assignationManager.Create(assignation);

            _notificationManager.sendNotification("Asignación", "Nueva asignación", input.InspectorId);
            
            return resp;
           
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public void Delete(DeleteAssignationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var inspectorId = _assignationManager.GetAssignationById(input.Id).InspectorId;
            var ManagerId = _assignationManager.GetAssignationById(input.Id).Operation.ManagerId;


            _assignationManager.Delete(input.Id);
            _notificationManager.sendNotification("Asignación", "Eliminación de asignación", inspectorId);
            _notificationManager.sendNotification("Asignación", "Se elimino una asignacion en una de sus operaciones", ManagerId);

        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetAssignationOutput> GetAll()
        {
            var getAll = _assignationManager.GetAll().ToList();
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(getAll);
            return output;
        }

        public GetAssignationOutput GetAssignationById(GetAssignationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var getAssignation = _assignationManager.GetAssignationById(input.Id);
            GetAssignationOutput output = ObjectMapper.Map<GetAssignationOutput>(getAssignation);
            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetAssignationOutput> GetAssignmentsByUser(long userId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(userId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetAssignationOutput> GetAssignmentsByOperation(int operationId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(null, operationId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<UserDtoOperation> GetUsersByOperation(int operationId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<UserDtoOperation> output = ObjectMapper.Map<List<UserDtoOperation>>(_assignationManager.GetUsersByOperation(operationId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsByUser(long userId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsConfirmedByUser(long userId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId, true));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsPendingByUser(long userId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId, null, true));

            return output;
        }

        public IEnumerable<GetAssignationOutput> GetMyAssignments()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(userId));

            return output;
        }

        public IEnumerable<GetAssignationOutput> GetMyAssignmentsByOperation(int operationId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(userId, operationId));

            return output;
        }

        public IEnumerable<GetOperationOutput> GetMyOperations()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId));

            return output;
        }

        public IEnumerable<GetOperationOutput> GetMyOperationsConfirmed(int? operationStateId = null)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId, true).WhereIf(operationStateId.HasValue, oper => oper.OperationStateId == operationStateId));

            return output;
        }

        public IEnumerable<GetOperationOutput> GetMyOperationsPending()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId, null, true));

            return output;
        }

        public IEnumerable<GetOperationOutput> GetMyOperationsFinished()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsFinishedByUser(userId));

            return output;
        }

        public void RefuseAssignation(int AssignationId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            _assignationManager.RefuseAssignation(AssignationId);
        }
        public void AceptAssignation(int AssignationId)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            _assignationManager.ConfirmAssignation(AssignationId);
        }
    }
}
