using Abp.Application.Services;
using App.Caliset.Models.OperationTypes;
using App.Caliset.OperationTypes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.OperationTypes
{
    public class OperationTypeAppService : ApplicationService, IOperationTypeAppService
    {
        private readonly IOperationTypeManager _operationTypeManager;
        public OperationTypeAppService(IOperationTypeManager operationTypeManager)
        {
            _operationTypeManager = operationTypeManager;
        }

        public IEnumerable<GetOperationTypeOutput> GetAll()
        {
            var getAll = _operationTypeManager.GetAll().ToList();
            List<GetOperationTypeOutput> output = ObjectMapper.Map<List<GetOperationTypeOutput>>(getAll);
            return output;
        }

        public async Task Create(CreateOperationTypeInput input)
        {
            var operationType = ObjectMapper.Map<OperationType>(input);
            await _operationTypeManager.Create(operationType);
        }

        public void Delete(DeleteOperationTypeInput input)
        {
            _operationTypeManager.Delete(input.Id);
        }

        public GetOperationTypeOutput GetOperationTypeById(GetOperationTypeInput input)
        {
            var getOperationType = _operationTypeManager.GetOperationTypeById(input.Id);
            GetOperationTypeOutput output = ObjectMapper.Map<GetOperationTypeOutput>(getOperationType);
            return output;
        }

        public void Update(UpdateOperationTypeInput input)
        {
            var operationType = _operationTypeManager.GetOperationTypeById(input.Id);
            _operationTypeManager.Update(operationType);
        }
    }
}