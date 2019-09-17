using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Models.Locations
{
    public interface ILocationManager : IDomainService
    {
        IEnumerable<Location> GetAll();
        Location GetLocationById(int id);
        Task<Location> Create(Location entity);
        void Update(Location entity);
        void Delete(int id);
    }
}
