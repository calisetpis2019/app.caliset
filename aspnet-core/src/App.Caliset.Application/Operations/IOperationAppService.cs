using Abp.Application.Services;
using App.Caliset.Operations.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Operations
{
    public interface IOperationAppService: IApplicationService
    {
        IEnumerable<GetOperationOutput> GetAll();
        Task Create(CreateOperationInput input);
        void Update(UpdateOperationInput input);
        void Delete(DeleteOperationInput input);
        GetOperationOutput GetOperationById(GetOperationInput input);
    }
}
