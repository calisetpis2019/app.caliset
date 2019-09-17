using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Models.OperationStates
{
    public class OperationState : FullAuditedEntity
    {
        protected OperationState() { }

        [Required]
        public string Name { get; set; }
    }
}
