using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Caliset.Models.LocationRecord
{
    public class LocationRecord: FullAuditedEntity
    {
        public LocationRecord() { }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }

        public DateTime When { get; set; }
        public long InspectorId { get; set; }
        [ForeignKey("InspectorId")]
        public virtual User Inspector { get; set; }
    }
}
