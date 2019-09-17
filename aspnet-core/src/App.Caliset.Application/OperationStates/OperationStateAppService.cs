using Abp.Application.Services;
using App.Caliset.Models.OperationStates;
using App.Caliset.OperationStates.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task Create(CreateOperationStateInput input)
        {
            var operationState = ObjectMapper.Map<OperationState>(input);
            await _operationStateManager.Create(operationState);
        }

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

        public void Update(UpdateOperationStateInput input)
        {
            var operationState = _operationStateManager.GetOperationStateById(input.Id);

            ObjectMapper.Map(input, operationState);

            _operationStateManager.Update(operationState);
        }
    }
}