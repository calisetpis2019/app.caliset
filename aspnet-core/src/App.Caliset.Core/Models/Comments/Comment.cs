using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Comments
{
    [Table("Comments")]
    public class Comment: FullAuditedEntity
    {
        public string Commentary { get; set; }
    }
}
