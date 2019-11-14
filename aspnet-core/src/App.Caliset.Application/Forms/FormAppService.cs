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

        [AbpAuthorize(PermissionNames.Operador)]
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
        public IEnumerable<GetReducedFormOutput> GetAll()
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Error", "Por favor inicie sesión.");
            }
            var getAll = _formManager.GetAll().ToList();
            List<GetReducedFormOutput> output = ObjectMapper.Map<List<GetReducedFormOutput>>(getAll);
            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetReducedFormOutput> GetAllFormByOperation(int IdOperation)
        {
            List<GetReducedFormOutput> output = ObjectMapper.Map<List<GetReducedFormOutput>>(_operationManager.GetFormsByOperation(IdOperation));

            return output;
        }

        [AbpAuthorize(PermissionNames.Operador)]
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

        [AbpAuthorize(PermissionNames.Operador)]
        public IEnumerable<GetReducedFormOutput> AntiGetAllFormByOperation(int IdOperation)
        {
            List<GetReducedFormOutput> output = ObjectMapper.Map<List<GetReducedFormOutput>>(_operationManager.NoGetFormsByOperation(IdOperation));

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

        [AbpAuthorize(PermissionNames.Operador)]
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
