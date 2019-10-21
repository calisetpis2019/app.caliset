using Abp;
using Abp.Application.Services;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.Models.Notifications;
using App.Caliset.Models.UserDeviceTokens;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.UserDeviceTokens
{
    public class UserDeviceTokenAppService : ApplicationService, IUserDeviceTokensAppService
    {
        private readonly IUserDeviceTokenManager _userDeviceTokenManager;
        private readonly IAbpSession _abpSession;
        private readonly INotificationManager _notificationManager;

        public UserDeviceTokenAppService(IUserDeviceTokenManager userDeviceTokenManager, 
            INotificationManager notificationManager,IAbpSession abpSession)
        {
            _userDeviceTokenManager = userDeviceTokenManager;
            _abpSession = abpSession;
            _notificationManager = notificationManager;
        }

        public void AddDeviceToken(string input)
        {

            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            long userId = _abpSession.UserId.Value;

            UserDeviceToken udt = new UserDeviceToken
            {
                UserId = userId,
                DeviceToken = input
            };
            _userDeviceTokenManager.Create(udt);
        }

        public bool existDeviceToken(long UserId)
        {
            return (_userDeviceTokenManager.getById(UserId) != null );
          
        }

        public IEnumerable<UserDto> GetAllUserElegible()
        {
            var getAll= _userDeviceTokenManager.GetAllUserElegible().ToList();
            List<UserDto> output = ObjectMapper.Map<List<UserDto>>(getAll);
            return output;
        }

        public void SendNotification(string Title, string Text)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "No manda notificacion.");
            }
            long userId = _abpSession.UserId.Value;

            _notificationManager.sendNotification(Title, Text, userId);
        }



    }
}
