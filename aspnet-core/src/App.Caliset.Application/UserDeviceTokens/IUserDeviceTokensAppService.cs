using Abp.Application.Services;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.UserDeviceTokens
{
    public interface IUserDeviceTokensAppService : IApplicationService
    {
        void AddDeviceToken(string userDeviceToken);
        void SendNotification(string Title, string Text);
        bool existDeviceToken(long UserId);
        IEnumerable<UserDto> GetAllUserElegible();
    }
}
