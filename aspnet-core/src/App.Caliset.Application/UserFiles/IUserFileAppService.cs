using Abp.Application.Services;
using App.Caliset.UserFiles.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.UserFiles
{
    public interface IUserFileAppService : IApplicationService
    {
        IEnumerable<GetUserFileOutput> GetAllByUser(long IdUser);
        Task Create(CreateUserFileInput input);
        void Delete(int input);
        GetUserFileOutput GetUserFileById(int input);
    
    }
}
