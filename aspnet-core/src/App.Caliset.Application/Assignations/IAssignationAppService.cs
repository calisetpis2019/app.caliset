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
        Task<bool> Create(CreateAssignationInput input);
        void Delete(DeleteAssignationInput input);
        GetAssignationOutput GetAssignationById(GetAssignationInput input);

        //WEB
        IEnumerable<GetAssignationOutput> GetAssignmentsByUser(long UserId);
        IEnumerable<GetAssignationOutput> GetAssignmentsByOperation(int OperationId);
        IEnumerable<GetOperationOutput> GetOperationsByUser(long userId);
        IEnumerable<GetOperationOutput> GetOperationsConfirmedByUser(long userId);
        IEnumerable<GetOperationOutput> GetOperationsPendingByUser(long userId);

        //MOBILE
        IEnumerable<GetAssignationOutput> GetMyAssignments();
        IEnumerable<GetAssignationOutput> GetMyAssignmentsByOperation(int OperationId);
        IEnumerable<GetOperationOutput> GetMyOperations();
        IEnumerable<GetOperationOutput> GetMyOperationsConfirmed(int? operationStateId);
        IEnumerable<GetOperationOutput> GetMyOperationsPending();
        IEnumerable<GetOperationOutput> GetMyOperationsFinished();

        void AceptAssignation(int AssignationId);
        void RefuseAssignation(int AssignationId);
    }
}
