using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserDeviceTokens
{
    public interface IUserDeviceTokenManager : IDomainService
    {
        Task<UserDeviceToken> Create(UserDeviceToken DT);
        void Delete(int id);
    }
}
