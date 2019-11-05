using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Forms
{
    public interface IFormOperationManager : IDomainService
    {
        IEnumerable<FormOperation> GetAll();
        FormOperation GetFormOperationById(int id);
        Task<FormOperation> Create(FormOperation entity);
     
        void Delete(int id);
    }
}
