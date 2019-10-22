using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.HoursRecord.Dto
{
    public class UpdateHoursRecordInput
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long InspectorId { get; set; }
        public int OperationId { get; set; }

    }
}
