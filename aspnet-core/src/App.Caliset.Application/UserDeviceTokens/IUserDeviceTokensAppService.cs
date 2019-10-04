using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.UserDeviceTokens
{
    public interface IUserDeviceTokensAppService : IApplicationService
    {
        void AddDeviceToken(string input);
        void SendNotification(string Title, string Text);
    }
}
