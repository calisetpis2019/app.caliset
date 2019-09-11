using Abp.Domain.Entities;

namespace App.Caliset.Operations
{
    public class OperationType : Entity<int>
    {
        public string Name { get; set; }
    }
}
