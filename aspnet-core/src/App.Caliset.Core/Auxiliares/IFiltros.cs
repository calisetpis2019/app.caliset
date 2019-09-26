using Abp.Domain.Services;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Operations;
using System.Collections.Generic;

namespace App.Caliset.Auxiliares
{
    public interface IFiltros: IDomainService
    {
        List<Operation> FOperationsState(List<Operation> Operations, string State);

        List<Assignation> FAssignationUser(List<Assignation> Assignations, long IdUser);
    }
}
