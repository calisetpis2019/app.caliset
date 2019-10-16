using Abp.Application.Services;
using App.Caliset.Models.UserFile;
using App.Caliset.UserFiles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.UserFiles
{
    public class UserFileAppService : ApplicationService, IUserFileAppService
    {


        private readonly UserFileManager _userFileManager;



        public UserFileAppService(UserFileManager userFileManager)
        {
            _userFileManager = userFileManager;

        }


        public async Task Create(CreateUserFileInput input)
        {
            var UFile = ObjectMapper.Map<UserFile>(input);
            await _userFileManager.Create(UFile);
        }

        public void Delete(int input)
        {
            _userFileManager.Delete(input);
        }

        public IEnumerable<GetUserFileOutput> GetAllByUser(long IdUser)
        {
            var getAll = _userFileManager.GetAllByUser(IdUser).ToList();
            List<GetUserFileOutput> output = ObjectMapper.Map<List<GetUserFileOutput>>(getAll);
            return output;
        }

        public GetUserFileOutput GetUserFileById(int input)
        {
            var getUserFile = _userFileManager.GetUserFileById(input);
            GetUserFileOutput output = ObjectMapper.Map<GetUserFileOutput>(getUserFile);
            return output;
        }
    }
}
