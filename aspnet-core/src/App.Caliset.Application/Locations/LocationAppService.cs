using Abp.Application.Services;
using App.Caliset.Models.Locations;
using App.Caliset.Locations.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using App.Caliset.Authorization;

namespace App.Caliset.Locations
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly ILocationManager _locationManager;
        public LocationAppService(ILocationManager locationManager)
        {
            _locationManager = locationManager;
        }

        public IEnumerable<GetLocationOutput> GetAll()
        {
            var getAll = _locationManager.GetAll().ToList();
            List<GetLocationOutput> output = ObjectMapper.Map<List<GetLocationOutput>>(getAll);
            return output;
        }

        public async Task Create(CreateLocationInput input)
        {
            var location = ObjectMapper.Map<Location>(input);
            await _locationManager.Create(location);
        }

        public void Delete(DeleteLocationInput input)
        {
            _locationManager.Delete(input.Id);
        }

        public GetLocationOutput GetLocationById(GetLocationInput input)
        {
            var getLocation = _locationManager.GetLocationById(input.Id);
            GetLocationOutput output = ObjectMapper.Map<GetLocationOutput>(getLocation);
            return output;
        }

        public void Update(UpdateLocationInput input)
        {
            var location = _locationManager.GetLocationById(input.Id);
            ObjectMapper.Map(input, location);
            _locationManager.Update(location);
        }
    }
}