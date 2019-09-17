using Abp.Application.Services;
using App.Caliset.OperationTypes.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.OperationTypes
{
    public interface IOperationTypeAppService : IApplicationService
    {
        IEnumerable<GetOperationTypeOutput> GetAll();
        Task Create(CreateOperationTypeInput input);
        void Update(UpdateOperationTypeInput input);
        void Delete(DeleteOperationTypeInput input);
        GetOperationTypeOutput GetOperationTypeById(GetOperationTypeInput input);

    }
}
