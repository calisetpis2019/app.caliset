using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Assignations
{
    public class Assignation : FullAuditedEntity
    {
        public Assignation() { }

        [Required]
        public long InspectorId { get; set; }
        [ForeignKey("InspectorId")]
        public virtual User Inspector { get; set; }

        [Required]
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public DateTime DateFin { get; set; }

        public bool? Aware { get; set; }
    }
}
