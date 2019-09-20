using App.Caliset.Clients.Dto;
using App.Caliset.Comments.Dto;
using App.Caliset.Locations.Dto;
using App.Caliset.OperationStates.Dto;
using App.Caliset.OperationTypes.Dto;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Operations.Dto
{
    public class CreateOperationInput
    {
        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public  GetLocationInput IdLocation { get; set; }
        
        public  IEnumerable<GetCommentInput> Comments { get; set; }
        public  GetOperationTypeInput OperationType { get; set; }
        public  GetClientInput Nominador { get; set; }
        public  GetClientInput Cargador { get; set; }
        public  GetOperationStateInput OperationState { get; set; }
        
        //public  IEnumerable<GetSampleOutput> Samples { get; set; }
        public  IEnumerable<UserDtoOperation> Inspectors { get; set; }
    }
}
