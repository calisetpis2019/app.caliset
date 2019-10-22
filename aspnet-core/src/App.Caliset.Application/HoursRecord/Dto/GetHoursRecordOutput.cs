using App.Caliset.Operations.Dto;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.HoursRecord.Dto
{
    public class GetHoursRecordOutput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long InspectorId { get; set; }
       
        public virtual UserDto Inspector { get; set; }
        public int OperationId { get; set; }

        public virtual GetOperationOutput Operation { get; set; }
    }
}
