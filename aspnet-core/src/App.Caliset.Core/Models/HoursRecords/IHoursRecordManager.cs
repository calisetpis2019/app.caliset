using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.HoursRecords
{
    public interface IHoursRecordManager: IDomainService
    {
        IEnumerable<HourRecord> GetAll();
        IEnumerable<HourRecord> GetAllByUser(long idUser);
        IEnumerable<HourRecord> GetAllByOperation(int IdOper);
        HourRecord GetHoursRecordById(int id);
        Task<HourRecord> Create(HourRecord entity);
        void Update(HourRecord entity);
        void Delete(int id);

        IEnumerable<HourRecord> GetMyRecordsFiltered(long userId);
    }
}
