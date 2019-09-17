using Abp.AutoMapper;
using App.Caliset.Models.Locations;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Locations.Dto
{
    [AutoMapFrom(typeof(Location))]
    public class CreateLocationInput
    {
        [Required]
        public string Name { get; set; }
    }
}
