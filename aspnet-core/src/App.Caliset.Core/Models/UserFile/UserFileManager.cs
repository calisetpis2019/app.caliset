using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Linq.Extensions;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserFile
{
    public class UserFileManager : DomainService, IUserFileManager
    {

        private readonly IRepository<UserFile> _repositoryUserFile;

        public UserFileManager(IRepository<UserFile> repositoryUserFile)
        {
            _repositoryUserFile = repositoryUserFile;
        }

        public async Task<UserFile> Create(UserFile entity)
        {
            var UFile = _repositoryUserFile.FirstOrDefault(x => x.Id == entity.Id);
            if (UFile != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                return await _repositoryUserFile.InsertAsync(entity);

            }
        }

        public void Delete(int id)
        {
            var UFile = _repositoryUserFile.FirstOrDefault(x => x.Id == id);
            if (UFile == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryUserFile.Delete(UFile);
            }
        }

        public IEnumerable<UserFile> GetAllByUser(long UserId)
        {
            return _repositoryUserFile.GetAll().WhereIf(true, UF => UF.UserId == UserId);
        }

        public UserFile GetUserFileById(int id)
        {
            return _repositoryUserFile.Get(id);
        }

        public void Update(UserFile entity)
        {
            _repositoryUserFile.Update(entity);
        }
    }
}
