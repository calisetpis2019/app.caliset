using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using App.Caliset.Authorization;
using App.Caliset.Forms.Dto;
using App.Caliset.Models.Forms;
using App.Caliset.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Forms
{
    public class FormAppService : ApplicationService, IFormAppService
    {

        private readonly IFormManager _formManager;
        private readonly IAbpSession _abpSession;
        private readonly IOperationManager _operationManager;


        public FormAppService(IFormManager formManager , IAbpSession abpSession, IOperationManager operationManager)
        {
            _formManager = formManager;
            _abpSession = abpSession;
            _operationManager = operationManager;

        }

        public void AddFormToOperation(CreateFormOperationInput input)
        {

            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var FOperation = ObjectMapper.Map<FormOperation>(input);
            FOperation.Form = _formManager.GetFormById(input.FormId);
            FOperation.Operation = _operationManager.GetOperationById(input.OperationId);

            _operationManager.AddForm(FOperation);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public async Task Create(CreateFormInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var FormX = ObjectMapper.Map<Form>(input);
            await _formManager.Create(FormX);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Delete(int input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            _formManager.Delete(input);
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public IEnumerable<GetFormOutput> GetAll()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var getAll = _formManager.GetAll().ToList();
            List<GetFormOutput> output = ObjectMapper.Map<List<GetFormOutput>>(getAll);
            return output;
        }

        public IEnumerable<GetFormOutput> GetAllFormByOperation(int IdOperation)
        {
            List<GetFormOutput> output = ObjectMapper.Map<List<GetFormOutput>>(_operationManager.GetFormsByOperation(IdOperation));

            return output;
        }

        public GetFormOutput GetFormById(int input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var getForm = _formManager.GetFormById(input);
            GetFormOutput output = ObjectMapper.Map<GetFormOutput>(getForm);
            return output;
        }

        public IEnumerable<GetFormOutput> AntiGetAllFormByOperation(int IdOperation)
        {
            List<GetFormOutput> output = ObjectMapper.Map<List<GetFormOutput>>(_operationManager.NoGetFormsByOperation(IdOperation));

            return output;
        
        }

        [AbpAuthorize(PermissionNames.Administrador)]
        public void Update(UpdateFormInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }

            var FormX = _formManager.GetFormById(input.Id);
            ObjectMapper.Map(input, FormX);
             _formManager.Update(FormX);
        }

        public void AntiAddFormToOperation(CreateFormOperationInput input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var FOperation = ObjectMapper.Map<FormOperation>(input);
            _operationManager.AntiAddForm(FOperation);

        }
    }
}
