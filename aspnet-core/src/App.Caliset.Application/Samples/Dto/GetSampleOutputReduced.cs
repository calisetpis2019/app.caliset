using Abp.AutoMapper;
using App.Caliset.Models.Samples;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Samples.Dto
{
    [AutoMapFrom(typeof(Sample))]
    public class GetSampleOutputReduced
    {
        public int Id { get; set; }
        public string IdSample { get; set; }
        public string Comment { get; set; }
        public DateTime CreationTime { get; set; }
        public long InspectorId { get; set; }
        public virtual UserDtoOperation Inspector { get; set; }
    }
}
