using System;

namespace App.Caliset.LocationRecords
{
    public class UpdateLocationRecordInput
    {
        public int Id { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public DateTime When { get; set;  }
        public long InspectorId { get; set; }
   
    }
}