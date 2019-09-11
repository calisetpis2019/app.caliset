using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Operations.Dto
{
    [AutoMapFrom(typeof(OperationType))]
    public class OperationTypeDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
