using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Models.Comments
{
    [Table("Comments")]
    public class Comment: FullAuditedEntity
    {
        public string Commentary { get; set; }
    }
}
