using Abp.AutoMapper;
using App.Caliset.Models.OperationStates;

namespace App.Caliset.OperationStates.Dto
{
    [AutoMapFrom(typeof(OperationState))]
    public class GetOperationStateInput
    {
        public int Id { get; set; }
    }
}
