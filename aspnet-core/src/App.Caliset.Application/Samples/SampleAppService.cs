using Abp.Application.Services;
using Abp.Domain.Repositories;
using App.Caliset.Models.Operations;
using App.Caliset.Models.Samples;
using App.Caliset.Operations;
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
        private readonly OperationManager _operationManager;
  

        public SampleAppService(SampleManager sampleManager, OperationManager operationManager)
        {
            _sampleManager = sampleManager;
            _operationManager = operationManager;
 
        }


        public async Task<string> Create(CreateSampleInput input)
        {
            var oper = _operationManager.GetAll();
            var oper2 = oper.FirstOrDefault(x => x.Id == input.OperationId);
            int aux;

            if (oper2.Samples == null)
                aux = 0;
            else
                aux = oper2.Samples.Count();

            var Sample = ObjectMapper.Map<Sample>(input);
            Sample.IdSample = "Operation" + oper2.Id.ToString() + "Sample" + aux.ToString();

            
            await _sampleManager.Create(Sample);
            return Sample.IdSample;

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

        public GetSampleOutput GetSampleById(GetSampleInput input)
        {
            var getSample = _sampleManager.GetSampleById(input.Id);
            GetSampleOutput output = ObjectMapper.Map<GetSampleOutput>(getSample);
            return output;
        }

        public IEnumerable<GetSampleOutput> GetSamplesByOperation(int operationId)
        {
            List<GetSampleOutput> output = ObjectMapper.Map<List<GetSampleOutput>>(_sampleManager.GetSamplesByOperation(operationId));

            return output;
        }

      
    }
}
