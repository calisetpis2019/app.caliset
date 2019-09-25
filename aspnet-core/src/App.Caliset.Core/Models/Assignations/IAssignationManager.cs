using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Models.Assignations
{
    public interface IAssignationManager: IDomainService
    {
        IEnumerable<Assignation> GetAll();
        Assignation GetAssignationById(int id);
        Task<Assignation> Create(Assignation entity);
        void Update(Assignation entity);
        void Delete(int id);
    }
}

