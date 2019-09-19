using Abp.Application.Services;
using Abp.Domain.Repositories;
using App.Caliset.Models.Samples;
using App.Caliset.Samples.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Samples
{
    public class SampleAppService : ApplicationService, ISampleAppService
    {

        private readonly SampleManager _sampleManager;

        public SampleAppService(SampleManager sampleManager)
        {
            _sampleManager = sampleManager;
        }


        public async Task Create(CreateSampleInput input)
        {
            var Sample = ObjectMapper.Map<Sample>(input);
            await _sampleManager.Create(Sample);
        }

        public void Delete(DeleteSampleInput input)
        {
            _sampleManager.Delete(input.Id);
        }

        public IEnumerable<GetSampleOutput> GetAll()
        {
            var getAll = _sampleManager.GetAll().ToList();
            List<GetSampleOutput> output = ObjectMapper.Map<List<GetSampleOutput>>(getAll);
            return output;
        }

        public GetSampleOutput GetClientById(GetSampleInput input)
        {
            var getSample = _sampleManager.GetSampleById(input.Id);
            GetSampleOutput output = ObjectMapper.Map<GetSampleOutput>(getSample);
            return output;
        }
    }
}
