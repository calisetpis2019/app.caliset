using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Clients
{
    [Table("Clients")]
    public class Client: FullAuditedEntity
    {
        protected Client() { }
        [Required]
            public string Name { get; set; }
    }
}
