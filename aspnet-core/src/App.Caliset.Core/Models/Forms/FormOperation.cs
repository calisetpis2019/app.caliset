using Abp.Domain.Entities.Auditing;
using App.Caliset.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Forms
{
    [Table("FormOperations")]
    public class FormOperation : FullAuditedEntity
    {

        public  FormOperation() { }
        [Required]
        public int OperationId { get; set; }
        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }

        [Required]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }
    }
}
