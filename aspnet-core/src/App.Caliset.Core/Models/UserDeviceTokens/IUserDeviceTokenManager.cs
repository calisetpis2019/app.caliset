using Abp.Domain.Services;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserDeviceTokens
{
    public interface IUserDeviceTokenManager : IDomainService
    {
        Task<UserDeviceToken> Create(UserDeviceToken DT);
        string getById(long UserId);
        void Delete(int id);
        IEnumerable<User> GetAllUserElegible(); 
    }
}
