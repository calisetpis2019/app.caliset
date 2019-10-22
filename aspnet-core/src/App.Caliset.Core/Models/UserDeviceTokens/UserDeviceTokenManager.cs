using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserDeviceTokens
{
    public class UserDeviceTokenManager : DomainService, IUserDeviceTokenManager
    {

        private readonly IRepository<UserDeviceToken> _repositoryUserDeviceToken;
        private readonly UserManager _userManager;

        public UserDeviceTokenManager(IRepository<UserDeviceToken> repositoryUserDeviceToken, UserManager userManager)
        {
            _repositoryUserDeviceToken = repositoryUserDeviceToken;
            _userManager = userManager;
        }

        public async Task<UserDeviceToken> Create(UserDeviceToken DT)
        {
            {
                var udt = _repositoryUserDeviceToken.FirstOrDefault(x => x.UserId == DT.UserId);
                if (udt != null)
                {
                    _repositoryUserDeviceToken.Delete(udt);
                }
               
                try
                {
                    return await _repositoryUserDeviceToken.InsertAsync(DT);
                }
                catch
                {
                    throw new UserFriendlyException("Error agregando el DeviceToken del Usuario");

                }
            }
        }

        public void Delete(int id)
        {
            var udt = _repositoryUserDeviceToken.FirstOrDefault(x => x.Id == id);
            if (udt == null)
            {
                throw new UserFriendlyException("Error", "No existe cliente.");
            }
            else
            {
                _repositoryUserDeviceToken.Delete(udt);
            }
        }

        public IEnumerable<User> GetAllUserElegible()
        {
            var usersE = (from Users in _userManager.GetAll()
                              join UDT in this._repositoryUserDeviceToken.GetAll()
                              on Users.Id equals UDT.UserId
                              select Users).Distinct();

            return usersE;
        }




        public string getById(long UserId)
        {
            try {
                string tk = _repositoryUserDeviceToken.FirstOrDefault(x => x.UserId == UserId).DeviceToken;
                return tk;
                    }
            catch
            {
                return null;
            }
        }

    }
}
