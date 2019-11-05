using Abp.AutoMapper;
using App.Caliset.Models.HoursRecords;
using App.Caliset.Operations.Dto;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.HoursRecord.Dto
{
    [AutoMapFrom(typeof(HourRecord))]
    public class GetHoursRecordOutput
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       
        public virtual UserDtoOperation Inspector { get; set; }


        public virtual ReduceOperationOutput Operation { get; set; }
    }
}
