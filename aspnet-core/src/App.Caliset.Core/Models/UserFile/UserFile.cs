using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.UserFile
{
    [Table("UserFiles")]
    public class UserFile : FullAuditedEntity
    {
        protected UserFile() { }
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Photo { get; set; }

    }
}
