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
    }
}
