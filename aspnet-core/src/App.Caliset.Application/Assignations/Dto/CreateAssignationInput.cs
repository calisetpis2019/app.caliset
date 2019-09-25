using Abp.AutoMapper;
using App.Caliset.Models.Assignations;
using System;

namespace App.Caliset.Assignations.Dto
{
    [AutoMapFrom(typeof(Assignation))]
    public class CreateAssignationInput
    {
        public int OperationId { get; set; }
        public int InspectorId { get; set; }
        public DateTime Date { get; set; }
        public bool Aware { get; set; }
    }
}
