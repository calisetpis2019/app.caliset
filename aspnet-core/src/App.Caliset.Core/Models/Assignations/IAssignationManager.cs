using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Models.Assignations
{
    public interface IAssignationManager: IDomainService
    {
        IEnumerable<Assignation> GetAll();
        Assignation GetAssignationById(int id);

        Task<bool> Create(Assignation entity);
        void Delete(int id);
        void ConfirmAssignation(int idAssignation);
        void RefuseAssignation(int idAssignation);
        void Notify(int idAssignation);
        bool UserAssigned(long IdUser, int IdOper);
    }
}

