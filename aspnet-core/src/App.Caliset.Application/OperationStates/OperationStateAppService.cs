﻿using Abp.Application.Services;
using Abp.Authorization;
using App.Caliset.Authorization;
using App.Caliset.Models.OperationStates;
using App.Caliset.OperationStates.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.OperationStates
{
    public class OperationStateAppService : ApplicationService, IOperationStateAppService
    {
        private readonly IOperationStateManager _operationStateManager;
        public OperationStateAppService(IOperationStateManager operationStateManager)
        {
            _operationStateManager = operationStateManager;
        }

        public IEnumerable<GetOperationStateOutput> GetAll()
        {
            var getAll = _operationStateManager.GetAll().ToList();
            List<GetOperationStateOutput> output = ObjectMapper.Map<List<GetOperationStateOutput>>(getAll);
            return output;
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public async Task Create(CreateOperationStateInput input)
        {
            var operationState = ObjectMapper.Map<OperationState>(input);
            await _operationStateManager.Create(operationState);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Delete(DeleteOperationStateInput input)
        {
            _operationStateManager.Delete(input.Id);
        }

        public GetOperationStateOutput GetOperationStateById(GetOperationStateInput input)
        {
            var getOperationState = _operationStateManager.GetOperationStateById(input.Id);
            GetOperationStateOutput output = ObjectMapper.Map<GetOperationStateOutput>(getOperationState);
            return output;
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Update(UpdateOperationStateInput input)
        {
            var operationState = _operationStateManager.GetOperationStateById(input.Id);

            ObjectMapper.Map(input, operationState);

            _operationStateManager.Update(operationState);
        }
    }
}