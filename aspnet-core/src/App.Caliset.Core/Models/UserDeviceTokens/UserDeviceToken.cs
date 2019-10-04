using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.UserDeviceTokens
{
    public class UserDeviceToken : FullAuditedEntity
    {
        public UserDeviceToken() { }
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public string DeviceToken { get; set; }

    }
}
