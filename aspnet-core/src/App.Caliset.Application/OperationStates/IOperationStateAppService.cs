using Abp.Application.Services;
using App.Caliset.OperationStates.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.OperationStates
{
    public interface IOperationStateAppService : IApplicationService
    {
        IEnumerable<GetOperationStateOutput> GetAll();
        Task Create(CreateOperationStateInput input);
        void Update(UpdateOperationStateInput input);
        void Delete(DeleteOperationStateInput input);
        GetOperationStateOutput GetOperationStateById(GetOperationStateInput input);

    }
}
