using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Models.OperationTypes
{
    public class OperationType : FullAuditedEntity
    {
        protected OperationType() { }

        [Required]
        public string Name { get; set; }
    }
}
