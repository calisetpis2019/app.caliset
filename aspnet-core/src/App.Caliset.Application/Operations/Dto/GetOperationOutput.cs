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
        //public virtual IEnumerable<GetCommentOutput> Comments { get; set; }
        public  GetOperationTypeOutput OperationType { get; set; }
        public  GetClientOutput Nominador { get; set; }
        public  GetClientOutput Cargador { get; set; }
        public  GetOperationStateOutput OperationState { get; set; }
       // public  IEnumerable<GetSampleOutput> Samples { get; set; }
       // public  IEnumerable<UserDtoOperation> Inspectors { get; set; }
    }
}
