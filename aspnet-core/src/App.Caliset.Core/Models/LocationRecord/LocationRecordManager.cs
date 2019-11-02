using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.LocationRecord
{
    public class LocationRecordManager : DomainService, ILocationRecordManager
    {


        private readonly IRepository<LocationRecord> _repositoryLocationRecord;
        private readonly UserManager _userManager;

        public LocationRecordManager(IRepository<LocationRecord> repositoryLocationRecord, UserManager userManager)
        {
            _repositoryLocationRecord = repositoryLocationRecord;
            _userManager = userManager;
        }

        public async Task<LocationRecord> Create(LocationRecord entity)
        {
            var LRecord = _repositoryLocationRecord.FirstOrDefault(x => x.Id == entity.Id);
            if (LRecord != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                return await _repositoryLocationRecord.InsertAsync(entity);

            }
        }

        public void Delete(int id)
        {
            var LRecord = _repositoryLocationRecord.FirstOrDefault(x => x.Id == id);
            if (LRecord == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryLocationRecord.Delete(LRecord);
            }
        }

        public IEnumerable<LocationRecord> GetAll()
        {
            return _repositoryLocationRecord.GetAll()
              .Include(x => x.Inspector)
          
              ;
        }


        public LocationRecord GetLocationRecordById(int id)
        {
            var Lrecord = _repositoryLocationRecord.Get(id);

            Lrecord.Inspector = _userManager.FindByIdAsync(Lrecord.InspectorId.ToString()).Result;

            return Lrecord;
        }

        public void Update(LocationRecord entity)
        {
            _repositoryLocationRecord.Update(entity);
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
