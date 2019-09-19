using Abp.AutoMapper;
using App.Caliset.Models.Samples;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Samples.Dto
{
    [AutoMapFrom(typeof(Sample))]
    public class CreateSampleInput
    {
        public string Comment { get; set; }
    }
}
