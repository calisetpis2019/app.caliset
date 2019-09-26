using Abp.Domain.Entities.Auditing;
using App.Caliset.Models.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Samples
{
    [Table("Samples")]
    public class Sample : FullAuditedEntity
    {
        protected Sample() {}

        public string IdSample { get; set; }
        public string Comment { get; set; }

        [Required]
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }

    }
}
