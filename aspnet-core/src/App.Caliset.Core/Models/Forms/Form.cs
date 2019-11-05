using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Forms
{
    [Table("Forms")]
    public class Form : FullAuditedEntity
    {
        protected Form() { }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Photo { get; set; }
        public string PathCompleto { get; set; }

        
    }
}
