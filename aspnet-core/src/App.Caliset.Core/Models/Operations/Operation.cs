using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Clients;
using App.Caliset.Models.Comments;
using App.Caliset.Models.HoursRecords;
using App.Caliset.Models.Locations;
using App.Caliset.Models.OperationStates;
using App.Caliset.Models.OperationTypes;
using App.Caliset.Models.Samples;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Operations
{
    public class Operation:FullAuditedEntity
    {
        public Operation() {}

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Commodity { get; set; }

        [Required]
        public string Package { get; set; }

        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string ClientReference { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public string Notes { get; set; }

        [Required]
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location{ get; set; }

        [Required]
        public int OperationTypeId { get; set; }
        [ForeignKey("OperationTypeId")]
        public virtual OperationType OperationType { get; set; }

        [Required]
        public int NominatorId { get; set; }
        [ForeignKey("NominatorId")]
        public virtual Client Nominator { get; set; }

        [Required]
        public int ChargerId { get; set; }
        [ForeignKey("ChargerId")]
        public virtual Client Charger { get; set; }

        [Required]
        public int OperationStateId { get; set; }
        [ForeignKey("OperationStateId")]
        public virtual OperationState OperationState { get; set; }

        [Required]
        public long ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual User Manager { get; set; }

        public virtual ICollection<Assignation> Assignations { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Sample> Samples { get; set; }

        public virtual IEnumerable<HourRecord> HoursRecord { get; set; }

    }

    
}
