using Abp.Application.Services;
using App.Caliset.Samples.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.LocationRecords
{
    public interface ILocationRecordAppService: IApplicationService
    {

        IEnumerable<GetLocationRecordOutput> GetAll();
        Task Create(CreateLocationRecordInput input);
        void Delete(int IdLocationRecord);
        void Update(UpdateLocationRecordInput input);
        GetLocationRecordOutput GetLocationRecordById(int IdLocationRecord);
        IEnumerable<GetLocationRecordOutput> GetLocationRecordByUserAndTime(long  IdUser, DateTime begin, DateTime end);
    }
}
