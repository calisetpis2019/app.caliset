using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Samples
{
    [Table("Samples")]
    public class Sample : FullAuditedEntity
    {
        protected Sample() {}

        public string Comment { get; set; }

    }
}
