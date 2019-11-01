using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.LocationRecord
{
    public interface ILocationRecordManager: IDomainService
    {
        IEnumerable<LocationRecord> GetAll();
        LocationRecord GetLocationRecordById(int id);
        Task<LocationRecord> Create(LocationRecord entity);
        void Update(LocationRecord entity);
        void Delete(int id);
    }
}
