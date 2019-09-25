using Abp.Domain.Entities.Auditing;
using App.Caliset.Models.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Comments
{
    [Table("Comments")]
    public class Comment : FullAuditedEntity
    {
        [Required]
        public string Commentary { get; set; }

        [Required]
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }
    }
}
