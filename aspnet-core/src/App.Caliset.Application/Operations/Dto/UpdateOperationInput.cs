using App.Caliset.Clients.Dto;
using App.Caliset.Locations.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Operations.Dto
{
    public class UpdateOperationInput
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Commodity { get; set; }
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public GetLocationInput IdLocation { get; set; }
        public GetClientInput Nominador { get; set; }
        public GetClientInput Cargador { get; set; }
    }
}
