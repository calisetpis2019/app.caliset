using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Notifications
{
    public interface INotificationManager:IDomainService
    {
        void sendNotification(string Title, string Text, long UserId);
        void sendMessage(string Text, long UserId);
    }
}
