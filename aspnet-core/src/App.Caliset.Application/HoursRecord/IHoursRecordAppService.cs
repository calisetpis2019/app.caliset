using Abp.Application.Services;
using App.Caliset.HoursRecord.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.HoursRecord
{
    public interface IHoursRecordAppService: IApplicationService
    {
        IEnumerable<GetHoursRecordOutput> GetAll();
        IEnumerable<GetHoursRecordOutput> GetAllByUser(long IdUser);
        IEnumerable<GetHoursRecordOutput> GetAllByOperation(int idOper);
        Task Create(CreateHoursRecordInput input);
        void Update(UpdateHoursRecordInput input);
        void Delete(int idHoursRecord);
        GetHoursRecordOutput GetHoursRecordById(int idHoursRecord);
    }
}
