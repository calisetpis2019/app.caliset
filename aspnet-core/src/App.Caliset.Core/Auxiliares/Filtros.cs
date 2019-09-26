using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Services;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.Operations;
using App.Caliset.Models.OperationStates;

namespace App.Caliset.Auxiliares
{
    public class Filtros :DomainService, IFiltros
    {
        public List<Assignation> FAssignationUser(List<Assignation> Assignations, long IdUser)
        {
            List<Assignation> ret = Assignations;

            for (var i = 0; i < Assignations.Count; i++)
            {
                if (Assignations[i].Inspector.Id != IdUser)
                {
                    ret.Remove(Assignations[i]);
                }
            }

            return ret;
        }

        public List<Operation> FOperationsState(List<Operation> Operations, string State)
        {

            List<Operation> ret = Operations;

            for (var i = 0; i < Operations.Count ; i++)
            {
                if( Operations[i].OperationState.Name != State)
                {
                    ret.Remove(Operations[i]);
                }
            }

            return ret;

        }


        
    }
}
