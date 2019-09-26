using Abp.Application.Services;
using App.Caliset.Assignations.Dto;
using App.Caliset.Operations.Dto;
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

        //WEB
        IEnumerable<GetAssignationOutput> GetAssignmentsByUser(long UserId);
        IEnumerable<GetAssignationOutput> GetAssignmentsByOperation(int OperationId);
        IEnumerable<GetOperationOutput> GetOperationsByUser(int userId);

        //MOBILE
        IEnumerable<GetAssignationOutput> GetMyAssignments();
        IEnumerable<GetAssignationOutput> GetMyAssignmentsByOperation(int OperationId);
        IEnumerable<GetOperationOutput> GetMyOperations();

        void AceptAssignation(int AssignationId);
        void RefuseAssignation(int AssignationId);
    }
}
