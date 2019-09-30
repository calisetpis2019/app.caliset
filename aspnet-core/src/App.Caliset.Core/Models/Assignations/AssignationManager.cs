﻿using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Caliset.Models.Assignations
{
    public class AssignationManager : DomainService, IAssignationManager
    {

        private readonly IRepository<Assignation> _repositoryAssignation;
        private readonly UserManager _userManager;
        private readonly OperationManager _operationManager;

        public AssignationManager(IRepository<Assignation> repositoryAssignation, UserManager userManager, OperationManager operationManager)
        {
            _repositoryAssignation = repositoryAssignation;
            _userManager = userManager;
            _operationManager = operationManager;
        }

        public async Task<Assignation> Create(Assignation entity)
        {
            var Assignation = _repositoryAssignation.FirstOrDefault(x => x.Id == entity.Id);
            if (Assignation != null)
            {
                throw new UserFriendlyException("Already Exist");
            }
            else
            {
                entity.Aware = null;
                return await _repositoryAssignation.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var Assignation = _repositoryAssignation.FirstOrDefault(x => x.Id == id);
            if (Assignation == null)
            {
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryAssignation.Delete(Assignation);
            }
        }

        public IEnumerable<Assignation> GetAll()
        {
            return _repositoryAssignation.GetAll().Include(asset => asset.Inspector)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.Location)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.OperationType)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.Nominator)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.Charger)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.OperationState)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.Manager)
                                                  .Include(asset => asset.Operation).ThenInclude(oper => oper.Samples);
        }

        public Assignation GetAssignationById(int id)
        {
            return _repositoryAssignation.Get(id);
        }

        public IEnumerable<Assignation> GetAssignmentsFilter(long? userId = null, int? operationId = null, DateTime? date = null, bool ? aware = null, bool ? pending = null)
        {
            return this.GetAll()
                    .WhereIf(userId.HasValue, assign => assign.InspectorId == userId)
                    .WhereIf(operationId.HasValue, assign => assign.OperationId == operationId)
                    .WhereIf(date.HasValue, assign => assign.Date == date)
                    .WhereIf(aware.HasValue, assign => assign.Aware == aware)
                    .WhereIf(pending.HasValue, assign => assign.Aware == null);
        }

        public IEnumerable<Operation> GetOperationsByUser(long userId, bool? aware = null, bool? pending = null)
        {
            var operations = (from Oper in _operationManager.GetCurrentOperations()
                              join Assign in this.GetAssignmentsFilter(userId, null, null, aware, pending)
                              on Oper.Id equals Assign.OperationId
                              select Oper).Distinct();

            return operations;
        }

        public void ConfirmAssignation(int idAssignation)
        {
            var Asign = _repositoryAssignation.FirstOrDefault(x => x.Id == idAssignation);
            Asign.Aware = true;
            _repositoryAssignation.Update(Asign);

        }

        public void RefuseAssignation(int idAssignation)
        {
            var Asign = _repositoryAssignation.FirstOrDefault(x => x.Id == idAssignation);
            Asign.Aware = false;
            _repositoryAssignation.Update(Asign);
        }

    }
}
