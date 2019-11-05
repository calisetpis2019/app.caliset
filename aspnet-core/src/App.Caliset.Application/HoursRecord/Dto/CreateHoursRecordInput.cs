using Abp.AutoMapper;
using App.Caliset.Models.HoursRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.HoursRecord.Dto
{
    [AutoMapFrom(typeof(HourRecord))]
    public class CreateHoursRecordInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int OperationId { get; set; }
    }
}
