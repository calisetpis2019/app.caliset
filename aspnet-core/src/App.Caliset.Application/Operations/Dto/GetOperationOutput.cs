using App.Caliset.Clients.Dto;
using App.Caliset.Comments.Dto;
using App.Caliset.Locations.Dto;
using App.Caliset.OperationStates.Dto;
using App.Caliset.OperationTypes.Dto;
using App.Caliset.Samples.Dto;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Operations.Dto
{
    public class GetOperationOutput
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public virtual GetLocationInput Location { get; set; }
        public virtual GetOperationTypeOutput OperationType { get; set; }
        public virtual GetClientOutput Nominator { get; set; }
        public virtual GetClientOutput Charger { get; set; }
        public virtual GetOperationStateOutput OperationState { get; set; }
        public virtual UserDtoOperation Manager { get; set; }

    }
}
