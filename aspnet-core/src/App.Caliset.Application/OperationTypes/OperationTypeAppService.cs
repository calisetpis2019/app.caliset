using Abp.Application.Services;
using Abp.Authorization;
using App.Caliset.Authorization;
using App.Caliset.Models.OperationTypes;
using App.Caliset.OperationTypes.Dto;
using System.Collections.Generic;
using System.Linq;
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

        [AbpAuthorize(PermissionNames.Administrador)]
        public async Task Create(CreateOperationTypeInput input)
        {
            var operationType = ObjectMapper.Map<OperationType>(input);
            await _operationTypeManager.Create(operationType);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
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

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Update(UpdateOperationTypeInput input)
        {
            var operationType = _operationTypeManager.GetOperationTypeById(input.Id);
            ObjectMapper.Map(input, operationType);
            _operationTypeManager.Update(operationType);
        }
    }
}