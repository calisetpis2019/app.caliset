using Abp.AutoMapper;
using App.Caliset.Models.OperationStates;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.OperationStates.Dto
{
    [AutoMapFrom(typeof(OperationState))]
    public class CreateOperationStateInput
    {
        [Required]
        public string Name { get; set; }
    }
}
