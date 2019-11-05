using Abp.Application.Services;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.HoursRecord.Dto;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.HoursRecords;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.HoursRecord
{
    public class HoursRecordAppService : ApplicationService, IHoursRecordAppService
    {


        private readonly IHoursRecordManager _hoursRecordManager;
        private readonly IAssignationManager _assignationManager;
        private readonly IAbpSession _abpSession;

        public HoursRecordAppService(IHoursRecordManager hoursRecordManager, IAbpSession abpSession
            , IAssignationManager assignationManager)
        {
            _hoursRecordManager = hoursRecordManager;
            _abpSession = abpSession;
            _assignationManager = assignationManager;
        }

        public async Task Create(CreateHoursRecordInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;


            HourRecord HRecord = ObjectMapper.Map<HourRecord>(input);
            HRecord.InspectorId = userId;

            if (_assignationManager.UserAssigned(userId, input.OperationId))
            {
                throw new UserFriendlyException("Error", "No se puede registrar horas en una operacion a la que no fue asignado.");
            }
            await _hoursRecordManager.Create(HRecord);
        }

        public void Delete(int idHoursRecord)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            _hoursRecordManager.Delete(idHoursRecord);
        }

        public IEnumerable<GetHoursRecordOutput> GetAll()
        {
            var getAll = _hoursRecordManager.GetAll().ToList();
            List<GetHoursRecordOutput> output = ObjectMapper.Map<List<GetHoursRecordOutput>>(getAll);
            return output;
        }

        public IEnumerable<GetHoursRecordOutput> GetAllByOperation(int idOper)
        {
            var getAll = _hoursRecordManager.GetAllByOperation(idOper).ToList();
            List<GetHoursRecordOutput> output = ObjectMapper.Map<List<GetHoursRecordOutput>>(getAll);
            return output;
        }

        public IEnumerable<GetHoursRecordOutput> GetAllByUser(long IdUser)
        {
            var getAll = _hoursRecordManager.GetAllByUser(IdUser).ToList();
            List<GetHoursRecordOutput> output = ObjectMapper.Map<List<GetHoursRecordOutput>>(getAll);
            return output;
        }

        public GetHoursRecordOutput GetHoursRecordById(int idHoursRecord)
        {
            var getHRecord = _hoursRecordManager.GetHoursRecordById(idHoursRecord);
            GetHoursRecordOutput output = ObjectMapper.Map<GetHoursRecordOutput>(getHRecord);
            return output;
        }

        public void Update(UpdateHoursRecordInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var HRecord = _hoursRecordManager.GetHoursRecordById(input.Id);
            ObjectMapper.Map(input, HRecord);
            _hoursRecordManager.Update(HRecord);
        }

        public IEnumerable<GetHoursRecordOutput> GetMyHoursRecordFiltered()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;
            List<GetHoursRecordOutput> output = ObjectMapper.Map<List<GetHoursRecordOutput>>(_hoursRecordManager.GetMyRecordsFiltered(userId));

            return output;
        }
    }
}
