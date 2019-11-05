using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Forms
{
    public interface IFormManager : IDomainService
    {
        IEnumerable<Form> GetAll();
        Form GetFormById(int id);
        Task<Form> Create(Form entity);
        void Update(Form entity);
        void Delete(int id);
    }
}
