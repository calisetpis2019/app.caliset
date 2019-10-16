using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserFile
{
    public interface IUserFileManager : IDomainService
    {
        IEnumerable<UserFile> GetAllByUser(long UserId);
        UserFile GetUserFileById(int id);
        Task<UserFile> Create(UserFile entity);
        void Update(UserFile entity);
        void Delete(int id);

    }
}
