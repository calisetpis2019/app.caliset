using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Forms
{
    public class FormManager : DomainService, IFormManager
    {


        private readonly IRepository<Form> _repositoryForm;
        const string Path = @"\Users\Emilio\Downloads\";

        public FormManager(IRepository<Form> repositoryForm)
        {
            _repositoryForm = repositoryForm;
        }

        public async Task<Form> Create(Form entity)
        {
            var FormX = _repositoryForm.FirstOrDefault(x => x.Id == entity.Id);
            if (FormX != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                string FileName = DateTime.Now.ToString().Replace(":", "").Replace("/", "");
                string PathCompleto = Path + FileName + ".jpg";
                File.WriteAllBytes(PathCompleto, entity.Photo);
                entity.PathCompleto = PathCompleto;
                entity.Photo = Encoding.ASCII.GetBytes("Insert Form here");
                return await _repositoryForm.InsertAsync(entity);

            }
        }

        public void Delete(int id)
        {
            var FormX = _repositoryForm.FirstOrDefault(x => x.Id == id);
            if (FormX == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                if ((System.IO.File.Exists(FormX.PathCompleto)))
                {
                    System.IO.File.Delete(FormX.PathCompleto);
                }
                else
                {
                    throw new UserFriendlyException("Error", "No existe el archivo");
                }
                _repositoryForm.Delete(FormX);
            }
        }

        public IEnumerable<Form> GetAll()
        {
            var ret = _repositoryForm.GetAll();

            foreach (var x in ret)
            {
                x.Photo = PhotoToByte(x.PathCompleto);

            }

            return ret;
        }

        public Form GetFormById(int id)
        {
            var ret = _repositoryForm.Get(id);
            ret.Photo = PhotoToByte(ret.PathCompleto);
            return ret;
        }

        public void Update(Form entity)
        {
            _repositoryForm.Update(entity);
        }
        public byte[] PhotoToByte(string Path)
        {
            return System.IO.File.ReadAllBytes(Path);
        }
    }
}
