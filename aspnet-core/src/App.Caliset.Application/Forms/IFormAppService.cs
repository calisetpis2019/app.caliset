﻿using Abp.Application.Services;
using App.Caliset.Forms.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Forms
{
    public interface IFormAppService : IApplicationService
    {
        IEnumerable<GetFormOutput> GetAll();
        Task Create(CreateFormInput input);
        void Update(UpdateFormInput input);
        void Delete(int input);
        GetFormOutput GetFormById(int input);
        void AddFormToOperation(CreateFormOperationInput input);
        IEnumerable<GetFormOutput> GetAllFormByOperation(int IdOperation);
    }
}