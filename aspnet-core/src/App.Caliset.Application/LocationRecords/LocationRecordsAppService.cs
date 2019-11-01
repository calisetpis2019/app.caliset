using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.Authorization;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.LocationRecord;
using App.Caliset.Samples.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.LocationRecords
{
    public class LocationRecordsAppService : ApplicationService, ILocationRecordAppService
    {

        private readonly UserManager _userManager;
        private readonly ILocationRecordManager _locationRecordManager;
        private readonly IAbpSession _abpSession;

        public LocationRecordsAppService(UserManager userManager, ILocationRecordManager locationRecordManager, IAbpSession abpSession)
        {
            _userManager = userManager;
            _locationRecordManager = locationRecordManager;
            _abpSession = abpSession;


        }

       public async Task Create(CreateLocationRecordInput input)
        {

            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;

            
            var LRecord = ObjectMapper.Map<LocationRecord>(input);

            LRecord.InspectorId = userId;
            LRecord.When = System.DateTime.Now;

            await _locationRecordManager.Create(LRecord);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Delete(int IdLocationRecord)
        {
            _locationRecordManager.Delete(IdLocationRecord);
        }

        public IEnumerable<GetLocationRecordOutput> GetAll()
        {
            var getAll = _locationRecordManager.GetAll().ToList();
            List<GetLocationRecordOutput> output = ObjectMapper.Map<List<GetLocationRecordOutput>>(getAll);

            return output;
        }

        public GetLocationRecordOutput GetLocationRecordById(int IdLocationRecord)
        {
            var Lrecord = _locationRecordManager.GetLocationRecordById(IdLocationRecord);
            var ret = ObjectMapper.Map<GetLocationRecordOutput>(Lrecord);

            return ret;
        }

        public IEnumerable<GetLocationRecordOutput> GetLocationRecordByUserAndTime(long IdUser, DateTime begin, DateTime end)
        {
            var LRecords = _locationRecordManager.GetAll().ToList()
                .Where(x => x.InspectorId == IdUser)
                .Where(x => x.When >= begin)
                .Where(x => x.When <= end)
               ;

            List<GetLocationRecordOutput> output = ObjectMapper.Map<List<GetLocationRecordOutput>>(LRecords);
            return output;
        }

        public void Update(UpdateLocationRecordInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var LRecord = _locationRecordManager.GetLocationRecordById(input.Id);
            ObjectMapper.Map(input, LRecord);
             _locationRecordManager.Update(LRecord);
        }
    }
}
