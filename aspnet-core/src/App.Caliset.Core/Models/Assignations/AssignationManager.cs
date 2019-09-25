using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
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
            return _repositoryAssignation.GetAll().Include(asset => asset.Operation).Include(asset => asset.Inspector);
        }

        public Assignation GetAssignationById(int id)
        {
            return _repositoryAssignation.Get(id);
        }

        public void Update(Assignation entity)
        {
            _repositoryAssignation.Update(entity);
        }

        public IEnumerable<Assignation> GetAssignmentsByUser(long userId)
        {   
            var assignatios = from Assign in this.GetAll()
                              where Assign.InspectorId == userId
                              select Assign;

            return assignatios;
        }

        public IEnumerable<Assignation> GetAssignmentsByOperation(int operationId)
        {
            var assignatios = from Assign in this.GetAll()
                              where Assign.OperationId == operationId
                              select Assign;

            return assignatios;
        }
        public IEnumerable<Assignation> GetAssignmentsByUserAndOperation(long userId, int operationId)
        {
            var assignatios = from Assign in this.GetAll()
                              where Assign.InspectorId == userId && Assign.OperationId == operationId
                              select Assign;

            return assignatios;
        }

        public IEnumerable<Operation> GetOperationsByUser(long userId)
        {
            var operations = from Oper in _operationManager.GetAll() 
                             join Assign in this.GetAll()
                             on Oper.Id equals Assign.OperationId
                             where Assign.Id == userId
                             select Oper;

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
