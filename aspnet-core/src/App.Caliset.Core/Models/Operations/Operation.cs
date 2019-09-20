using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Clients;
using App.Caliset.Models.Comments;
using App.Caliset.Models.Locations;
using App.Caliset.Models.OperationStates;
using App.Caliset.Models.OperationTypes;
using App.Caliset.Models.Samples;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.Operations
{
    [Table("Operations")]
    public class Operation:FullAuditedEntity
    {
        protected Operation() { }

        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public virtual Location Location{ get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual OperationType OperationType { get; set; }
        public virtual Client Nominador { get; set; }
        public virtual  Client Cargador { get; set; }
        public virtual OperationState OperationState { get; set; }
        public virtual IEnumerable<Sample> Samples { get; set; }
        public virtual IEnumerable<User> Inspectors { get; set; }

        //public User Operator { get; set; }
        // COMO ES FULLAUDITED YA AVISA QUIEN LO CREO

        //public User ResposableOperation {get; set; }


        //public IEnumerable<Form> Forms {get; set; }

        //public AssignmentData AssignmentDataUser {get; set;}

        //pubic IEnumerable<HoursRecord> HourRecords {get; set;}
    }
}
