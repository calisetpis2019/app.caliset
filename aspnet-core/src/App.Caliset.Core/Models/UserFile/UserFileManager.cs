using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Linq.Extensions;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.UserFile
{
    public class UserFileManager : DomainService, IUserFileManager
    {

        private readonly IRepository<UserFile> _repositoryUserFile;
        const string Path = @"\Users\Emilio\Downloads\";

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
                string FileName = DateTime.Now.ToString().Replace(":","").Replace("/","") ;
                string PathCompleto = Path + FileName + ".jpg";
                File.WriteAllBytes(PathCompleto, entity.Photo);
                entity.PathCompleto = PathCompleto;
                entity.Photo = Encoding.ASCII.GetBytes("Insert photo here");
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
                if ((System.IO.File.Exists(UFile.PathCompleto)))
                {
                    System.IO.File.Delete(UFile.PathCompleto);
                }
                else
                {
                    throw new UserFriendlyException("Error", "No existe el archivo");
                }
                _repositoryUserFile.Delete(UFile);
            }
        }

        public IEnumerable<UserFile> GetAllByUser(long UserId)
        {
            var ret = _repositoryUserFile.GetAll().WhereIf(true, UF => UF.UserId == UserId);
            
            foreach ( var x in ret)
            {
                x.Photo = PhotoToByte(x.PathCompleto);
            }

            return ret;
        }

        public UserFile GetUserFileById(int id)
        {
            var ret = _repositoryUserFile.Get(id);
            ret.Photo = PhotoToByte(ret.PathCompleto);
            return ret;
        }

        public void Update(UserFile entity)
        {
            _repositoryUserFile.Update(entity);
        }

        public byte[] PhotoToByte(string Path)
        {
            return System.IO.File.ReadAllBytes(Path);
        }
    }
}
