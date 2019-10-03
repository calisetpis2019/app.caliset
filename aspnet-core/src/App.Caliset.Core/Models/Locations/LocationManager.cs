using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.Models.Locations
{
    public class LocationManager : DomainService, ILocationManager
    {
        private readonly IRepository<Location> _repositoryLocation;
        public LocationManager(IRepository<Location> repositoryLocation)
        {
            _repositoryLocation = repositoryLocation;
        }

        public async Task<Location> Create(Location entity)
        {
            var location = _repositoryLocation.GetAll().Where(x => x.Id == entity.Id || x.Name == entity.Name);
            if (location.Count() > 0)
            {
                throw new UserFriendlyException("Error", "Ya existe ubicación.");
            }
            else
            {
                return await _repositoryLocation.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var location = _repositoryLocation.FirstOrDefault(x => x.Id == id);
            if (location == null)
            {
                throw new UserFriendlyException("Error", "No existe ubicación.");
            }
            else
            {
                _repositoryLocation.Delete(location);
            }
        }

        public IEnumerable<Location> GetAll()
        {
            return _repositoryLocation.GetAll();
        }


        public Location GetLocationById(int id)
        {
            return _repositoryLocation.Get(id);
        }

        public void Update(Location entity)
        {
            _repositoryLocation.Update(entity);
        }
    }
}
