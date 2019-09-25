using Abp.Application.Services;
using App.Caliset.Assignations.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Caliset.Assignations
{
    public interface IAssignationAppService: IApplicationService
    {
        IEnumerable<GetAssignationOutput> GetAll();
        Task Create(CreateAssignationInput input);
        void Delete(DeleteAssignationInput input);
        GetAssignationOutput GetAssignationById(GetAssignationInput input);

        IEnumerable<GetAssignationOutput> GetAssignmentsByOperation(int OperationId);
        IEnumerable<GetAssignationOutput> GetAssignmentsByUser(long UserId);

        IEnumerable<GetAssignationOutput> GetAssignments();
    }
}
