using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Samples
{
    [Table("Samples")]
    public class Sample : FullAuditedEntity
    {
        protected Sample() {}

        public string Comment { get; set; }

    }
}
