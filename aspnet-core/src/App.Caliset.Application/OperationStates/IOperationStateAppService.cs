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
        void Update(UpdateOperationStateInput input);
        GetOperationStateOutput GetOperationStateById(GetOperationStateInput input);

    }
}
