using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Models.Assignations
{
    public class Assignation : FullAuditedEntity
    {
        public Assignation() { }
        public virtual User Inspector { get; set; }
        public virtual Operation Operation { get; set; }
        public DateTime Date { get; set; }
        public bool Aware { get; set; }
    }
}
