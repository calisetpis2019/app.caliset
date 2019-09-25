using Abp.Domain.Entities.Auditing;
using App.Caliset.Models.Operations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Comments
{
    [Table("Comments")]
    public class Comment : FullAuditedEntity
    {
        public string Commentary { get; set; }

        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }
    }
}
