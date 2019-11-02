using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.Authorization;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.HoursRecords;
using App.Caliset.Models.LocationRecord;
using App.Caliset.Models.Locations;
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
        private readonly IHoursRecordManager _hoursRecordManager;
        private readonly IAbpSession _abpSession;
        private readonly ILocationManager _locationManager;

        public LocationRecordsAppService(UserManager userManager, ILocationRecordManager locationRecordManager, IAbpSession abpSession
            , IHoursRecordManager hoursRecordManager, ILocationManager locationManager)
        {
            _userManager = userManager;
            _locationRecordManager = locationRecordManager;
            _abpSession = abpSession;
            _hoursRecordManager = hoursRecordManager;
            _locationManager = locationManager;


        }

        public IEnumerable<CompareLocationOutput> ControlRecord(long IdUser, int IdHourRecord)
        {
            var HorasRegistradas = _hoursRecordManager.GetHoursRecordById(IdHourRecord);

            var LocationOperation = _locationManager.GetLocationById(HorasRegistradas.Operation.LocationId);
            var HorasReales = this.GetLocationRecordByUserAndTime(IdUser, HorasRegistradas.StartDate, HorasRegistradas.EndDate);
            List<CompareLocationOutput> resultado = new List<CompareLocationOutput>();

            double lat1;
            double lon1;
            double latLocation = Convert.ToDouble(LocationOperation.Latitude);
            double lonLocation = Convert.ToDouble(LocationOperation.Longitude);
            double Radio = Convert.ToDouble(LocationOperation.Radius);
            double Distancia;
            foreach (var CadaRegistro in HorasReales)
            {
                lon1 = Convert.ToDouble(CadaRegistro.Latitude);
                lat1 = Convert.ToDouble(CadaRegistro.Longitude);

                Distancia =_locationRecordManager.GetDistance(lon1, lat1, latLocation, lonLocation);
                CompareLocationOutput item = new CompareLocationOutput
                {
                    IsThere = Distancia < Radio,
                    LocationRegistrado = CadaRegistro
                };

                resultado.Add(item);

            }

            return resultado;
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
