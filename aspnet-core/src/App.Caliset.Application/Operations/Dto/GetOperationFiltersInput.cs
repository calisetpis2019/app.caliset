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
    public class GetOperationFiltersInput
    {
        /*public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string ClientReference { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public string Notes { get; set; }*/
        public int? LocationId { get; set; }
        public int? OperationTypeId { get; set; }
        public int? NominatorId { get; set; }
        public int? ChargerId { get; set; }
        public int? OperationStateId { get; set; }
        public int? ManagerId { get; set; }

    }
}
