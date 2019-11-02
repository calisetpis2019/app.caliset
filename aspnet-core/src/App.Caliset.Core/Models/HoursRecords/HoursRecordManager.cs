using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.HoursRecords
{
    public class HoursRecordManager : DomainService, IHoursRecordManager
    {


        private readonly IRepository<HourRecord> _hoursRecordRepository;
        private readonly IRepository<Operation> _operationRepository;

        public HoursRecordManager(IRepository<HourRecord> hoursRecordRepository, IRepository<Operation> operationRepository)
        {
            _hoursRecordRepository = hoursRecordRepository;
            _operationRepository = operationRepository;
        }


        public async Task<HourRecord> Create(HourRecord entity)
        {
            var hrecord = _hoursRecordRepository.FirstOrDefault(x => x.Id == entity.Id);
            if (hrecord != null)
            {
                throw new UserFriendlyException("Error", "Ya existe operación.");
            }
            else
            {
                if(entity.StartDate < entity.EndDate) 
                {
                    return await _hoursRecordRepository.InsertAsync(entity);
                }
                   
                else
                {
                    throw new UserFriendlyException("Error", "La fecha de inicio debe ser anterior a la fecha de fin");
                }
            }
        }

        public void Delete(int id)
        {
            var hrecord = _hoursRecordRepository.FirstOrDefault(x => x.Id == id);
            if (hrecord == null)
            {
                throw new UserFriendlyException("Error", "No existe el registro.");
            }
            else
            {
                _hoursRecordRepository.Delete(hrecord);
            }
        }

        public IEnumerable<HourRecord> GetAll()
        {
            return _hoursRecordRepository.GetAll()
                 .Include(x => x.Inspector)
                 .Include(x => x.Operation)
                 ;
        }

        public IEnumerable<HourRecord> GetAllByOperation(int IdOper)
        {
            return this.GetAll().Where(x => x.OperationId == IdOper);
        }

        public IEnumerable<HourRecord> GetAllByUser(long idUser)
        {
            return this.GetAll().Where(x => x.InspectorId == idUser);
        }

        public HourRecord GetHoursRecordById(int id)
        {
            var ret =  _hoursRecordRepository.FirstOrDefault(id);
           ret.Operation = _operationRepository.FirstOrDefault(ret.OperationId);
            return ret;
        }

        public void Update(HourRecord entity)
        {
            _hoursRecordRepository.Update(entity);
        }
    }
}
