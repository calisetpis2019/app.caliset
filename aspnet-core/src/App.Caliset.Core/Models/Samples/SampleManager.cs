using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System;
using System.Collections.Generic;
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


        public async Task<Sample> Create(Sample entity)
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
            return _repositorySample.GetAll();
        }

        public Sample GetSampleById(int id)
        {
            return _repositorySample.Get(id);
        }

        public void Update(Sample entity)
        {
            _repositorySample.Update(entity);
        }
    }
}
