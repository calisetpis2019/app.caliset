using Abp.Application.Services;
using Abp.Domain.Repositories;
using App.Caliset.Operations.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Operations
{
    public class OperationTypeAppService : CrudAppService<OperationType, OperationTypeDto>
    {
        public OperationTypeAppService(IRepository<OperationType, int> repository) : base(repository)
        {
        }
    }
}
