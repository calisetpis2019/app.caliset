using Abp.Application.Services;
using App.Caliset.Locations.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Locations
{
    public interface ILocationAppService : IApplicationService
    {
        IEnumerable<GetLocationOutput> GetAll();
        Task Create(CreateLocationInput input);
        void Update(UpdateLocationInput input);
        void Delete(DeleteLocationInput input);
        GetLocationOutput GetLocationById(GetLocationInput input);

    }
}
