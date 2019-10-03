using Abp.Application.Services;
using App.Caliset.Clients.Dto;
using App.Caliset.Samples.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Samples
{
    public interface ISampleAppService: IApplicationService
    {
        IEnumerable<GetSampleOutput> GetAll();
        Task<string> Create(CreateSampleInput input);
        void Delete(DeleteSampleInput input);
        GetSampleOutput GetSampleById(GetSampleInput input);
        IEnumerable<GetSampleOutput> GetSamplesByOperation(int operationId);
        //Task<string> AddSampleOperation(GetSampleInput input);
    }
}
