using Abp.Application.Services;
using App.Caliset.Models.Locations;
using App.Caliset.Locations.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using App.Caliset.Authorization;
using Abp.Runtime.Session;
using Abp.UI;

namespace App.Caliset.Locations
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly ILocationManager _locationManager;
        private readonly IAbpSession _abpSession;
        public LocationAppService(ILocationManager locationManager,   IAbpSession abpSession)
        {
            _locationManager = locationManager;
            _abpSession = abpSession;
        }

        public IEnumerable<GetLocationOutput> GetAll()
        {
            var getAll = _locationManager.GetAll().ToList();
            List<GetLocationOutput> output = ObjectMapper.Map<List<GetLocationOutput>>(getAll);
            return output;
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public async Task Create(CreateLocationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var location = ObjectMapper.Map<Location>(input);
            await _locationManager.Create(location);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Delete(DeleteLocationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }

            _locationManager.Delete(input.Id);
        }

        public GetLocationOutput GetLocationById(GetLocationInput input)
        {
            var getLocation = _locationManager.GetLocationById(input.Id);
            GetLocationOutput output = ObjectMapper.Map<GetLocationOutput>(getLocation);
            return output;
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Update(UpdateLocationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }

            var location = _locationManager.GetLocationById(input.Id);
            ObjectMapper.Map(input, location);
            _locationManager.Update(location);
        }
    }
}