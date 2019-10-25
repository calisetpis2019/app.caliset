using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
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
        private readonly IAbpSession _abpSession;


        public SampleAppService(SampleManager sampleManager, OperationManager operationManager, IAbpSession abpSession)
        {
            _sampleManager = sampleManager;
            _operationManager = operationManager;
            _abpSession = abpSession;


        }


        public async Task<string> Create(CreateSampleInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;


            var oper2 = _operationManager.GetOperationById( input.OperationId);
     
            int aux;

            if (oper2.Samples == null)
                aux = 0;
            else
                aux = oper2.Samples.Count();

            var Sample = ObjectMapper.Map<Sample>(input);
            Sample.InspectorId = userId;
            Sample.IdSample = "Operation" + oper2.Id.ToString() + "Sample" + aux.ToString();

            
            await _sampleManager.Create(Sample);
            return Sample.IdSample;

        }

        public void Delete(DeleteSampleInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }

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
            var getSample = _sampleManager.GetSampleByIdAsync(input.Id);
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
