using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using App.Caliset.Authorization.Users;

namespace App.Caliset.Operations
{
    public class OperationType : Entity<int>, IFullAudited<User>, ISoftDelete
    {
        public string Name { get; set; }
        public User CreatorUser { get; set; }
        public User LastModifierUser { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public User DeleterUser { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
