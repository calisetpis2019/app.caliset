using App.Caliset.HoursRecord.Dto;
using System.Collections.Generic;

namespace App.Caliset.Operations.Dto
{
    public class GetOperationHourRecordOutput
    {
        public int Id { get; set; }
        
        public List<GetHoursRecordOutput> HoursRecord { get; set; }

    }
}
