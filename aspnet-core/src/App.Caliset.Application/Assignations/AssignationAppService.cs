﻿using Abp.Application.Services;
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

namespace App.Caliset.Assignations
{
    public class AssignationAppService : ApplicationService, IAssignationAppService
    {

        private readonly AssignationManager _assignationManager;
        private readonly IAbpSession _abpSession;



        public AssignationAppService(AssignationManager assignationManager, IAbpSession abpSession)
        {
            _assignationManager = assignationManager;
            _abpSession = abpSession;
 
        }


        [AbpAuthorize(PermissionNames.Operador)]
        public async Task Create(CreateAssignationInput input)
        {
            var assignation = ObjectMapper.Map<Assignation>(input);

            await _assignationManager.Create(assignation);
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public void Delete(DeleteAssignationInput input)
        {
            _assignationManager.Delete(input.Id);
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
            var getAssignation = _assignationManager.GetAssignationById(input.Id);
            GetAssignationOutput output = ObjectMapper.Map<GetAssignationOutput>(getAssignation);
            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetAssignationOutput> GetAssignmentsByUser(long userId)
        {
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(userId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetAssignationOutput> GetAssignmentsByOperation(int operationId)
        {
            List<GetAssignationOutput> output = ObjectMapper.Map<List<GetAssignationOutput>>(_assignationManager.GetAssignmentsFilter(null, operationId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsByUser(long userId)
        {
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsConfirmedByUser(long userId)
        {
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(_assignationManager.GetOperationsByUser(userId, true));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetOperationOutput> GetOperationsPendingByUser(long userId)
        {
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

        public void RefuseAssignation(int AssignationId)
        {
            _assignationManager.RefuseAssignation(AssignationId);
        }
        public void AceptAssignation(int AssignationId)
        {
            _assignationManager.ConfirmAssignation(AssignationId);
        }
    }
}
