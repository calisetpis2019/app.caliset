using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Samples
{
    public class SampleManager : DomainService, ISampleManager
    {

        private readonly IRepository<Sample> _repositorySample;

        public SampleManager(IRepository<Sample> repositorySample)
        {
            _repositorySample = repositorySample;
        }


         public async  Task<Sample> Create(Sample entity)
        {
            
            var Sample = _repositorySample.FirstOrDefault(x => x.Id == entity.Id);
            if (Sample != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                return await _repositorySample.InsertAsync(entity);
               
            }
        }

        public void Delete(int id)
        {
            var Sample = _repositorySample.FirstOrDefault(x => x.Id == id);
            if (Sample == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositorySample.Delete(Sample);
            }
        }

        public IEnumerable<Sample> GetAll()
        {
            return _repositorySample.GetAll()
                .Include(x => x.Inspector)
                .Include(x => x.Operation)
                ;

        }

        public  Sample GetSampleByIdAsync(int id)
        {
            return  _repositorySample.Get(id);
        }

        public void Update(Sample entity)
        {
            _repositorySample.Update(entity);
        }

        public IEnumerable<Sample> GetSamplesByOperation(int operationId)
        {
            var samples = from Sample in this.GetAll()
                           where Sample.OperationId == operationId
                           select Sample;

            return samples;
        }

        public string AddSampleToOperation(string IdSample, Operation Oper)
        {
            var sample = _repositorySample.FirstOrDefault(x => x.IdSample == IdSample);
            Oper.Samples.Append(sample);
            return sample.IdSample;
        }
    }
}
