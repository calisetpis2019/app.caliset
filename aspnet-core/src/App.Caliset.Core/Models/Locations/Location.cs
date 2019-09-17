using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Models.Locations
{
    public class Location : FullAuditedEntity
    {
        protected Location() { }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        [Required]
        public float Radius { get; set; }
    }
}
