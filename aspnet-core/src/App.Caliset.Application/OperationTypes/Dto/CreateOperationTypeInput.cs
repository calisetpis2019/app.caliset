using Abp.AutoMapper;
using App.Caliset.Models.OperationTypes;
using System.ComponentModel.DataAnnotations;

namespace App.Caliset.OperationTypes.Dto
{
    [AutoMapFrom(typeof(OperationType))]
    public class CreateOperationTypeInput
    {
        [Required]
        public string Name { get; set; }
    }
}
