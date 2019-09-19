using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Samples
{
    public interface ISampleManager: IDomainService
    {
        IEnumerable<Sample> GetAll();
        Sample GetSampleById(int id);
        Task<Sample> Create(Sample entity);
        void Update(Sample entity);
        void Delete(int id);
    }
}

