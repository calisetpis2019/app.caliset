using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Forms
{
    public class FormOperationManager : DomainService, IFormOperationManager
    {

        private readonly IRepository<FormOperation> _repositoryFormOperation;
        private readonly IRepository<Form> _repositoryForm;
        private readonly IRepository<Operation> _repositoryOperation;

        public FormOperationManager(IRepository<Form> repositoryForm, IRepository<FormOperation> repositoryFormOperation, IRepository<Operation> repositoryOperation)
        {
            _repositoryForm = repositoryForm;
            _repositoryFormOperation = repositoryFormOperation;
            _repositoryOperation = repositoryOperation;
        }

        public async Task<FormOperation> Create(FormOperation entity)
        {
            var FO = _repositoryFormOperation.GetAll().Where(x => x.Id == entity.Id || ( x.OperationId == entity.OperationId && x.FormId == entity.FormId));
            if (FO.Count() > 0)
            {
                throw new UserFriendlyException("Error", "Ya se adjunto ese Formulario a la operacion.");
            }
            else
            {
                return await _repositoryFormOperation.InsertAsync(entity);
            }
        }

        public  void Delete(int id)
        {
            var FO = _repositoryFormOperation.FirstOrDefault(id);
            if (FO != null)
            {
                throw new UserFriendlyException("Error", "Ya se adjunto ese Formulario a la operacion.");
            }
            else
            {
                _repositoryFormOperation.Delete(id);
            }
        }

        public IEnumerable<FormOperation> GetAll()
        {
            return _repositoryFormOperation.GetAll()
                .Include(x => x.Operation)
                .Include(x => x.Form);
        }

        public FormOperation GetFormOperationById(int id)
        {


            var ret = _repositoryFormOperation.FirstOrDefault(id);
            ret.Form = _repositoryForm.FirstOrDefault(ret.FormId);
            ret.Operation = _repositoryOperation.FirstOrDefault(ret.OperationId);

            return ret; 
        }

      
    }
}
