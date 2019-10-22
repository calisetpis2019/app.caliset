using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.HoursRecords
{
    [Table("HoursRecord")]
    public class HourRecord : FullAuditedEntity 
    {
        protected HourRecord() { }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long InspectorId { get; set; }
        [ForeignKey("InspectorId")]
        public virtual User Inspector { get; set; }
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }
    }
}
