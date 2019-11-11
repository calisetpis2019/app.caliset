using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Mails
{
    public interface IMailManager: IDomainService
    {
        void SendMail(string to, string subject, string body);
    }
}
