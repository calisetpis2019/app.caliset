using App.Caliset.Authorization.Users;
using App.Caliset.Users.Dto;
using System;

namespace App.Caliset.LocationRecords
{
    public class GetLocationRecordOutput
    {
        public int Id { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public DateTime When { get; set; }
        public long InspectorId { get; set; }

        public virtual UserDtoOperation Inspector { get; set; }
    }
}