using Abp.Application.Services;
using App.Caliset.Models.Operations;
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
        public OperationAppService(IOperationManager operationManager)
        {
            _operationManager = operationManager;
        }



        public async Task Create(CreateOperationInput input)
        {
            input.Inspectors = null;
            input.Comments = null;
            var Oper = ObjectMapper.Map<Operation>(input);
            await _operationManager.Create(Oper);
        }

        public void Delete(DeleteOperationInput input)
        {
            _operationManager.Delete(input.Id);
        }

        public IEnumerable<GetOperationOutput> GetAll()
        {
            var getAll = _operationManager.GetAll().ToList();
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(getAll);
            return output;
        }

        public GetOperationOutput GetOperationById(GetOperationInput input)
        {
            var Oper = _operationManager.GetAll().FirstOrDefault(x => x.Id == input.Id);
            var ret = ObjectMapper.Map<GetOperationOutput>(Oper);

            return ret;
        }

        public void Update(UpdateOperationInput input)
        {
            var Opert = _operationManager.GetOperationById(input.Id);
            ObjectMapper.Map(input, Opert);
            _operationManager.Update(Opert);
        }
    }
}
