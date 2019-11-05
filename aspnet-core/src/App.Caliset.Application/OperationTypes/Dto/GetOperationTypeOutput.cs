using Abp.AutoMapper;
using App.Caliset.Models.OperationTypes;

namespace App.Caliset.OperationTypes.Dto
{
    [AutoMapFrom(typeof(OperationType))]
    public class GetOperationTypeOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
