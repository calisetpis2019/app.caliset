using Abp.Domain.Services;
using App.Caliset.Models.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Samples
{
    public interface ISampleManager: IDomainService
    {
        IEnumerable<Sample> GetAll();
        Sample GetSampleByIdAsync(int id);
        Task<Sample> Create(Sample entity);
        void Update(Sample entity);
        void Delete(int id);
        string AddSampleToOperation(string IdSample, Operation Oper);
    }
}

