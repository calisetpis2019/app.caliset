using Abp.Application.Services;
using App.Caliset.Operations.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Operations
{
    public interface IOperationAppService: IApplicationService
    {
        IEnumerable<GetOperationOutput> GetAll();
        Task Create(CreateOperationInput input);
        void Update(UpdateOperationInput input);
        // SOLO PERMITE EL CAMBIO DE LOS CAMPOS: 
            //.Location
            //.Nominador 
            //.Cargador 
            //.OperationType
            //.Date 
            //.Commodity 
            //.Package 
            //.ShipName 
            //.Destiny 
            //.Line 
            //.BookingNumber 
                
        void Delete(DeleteOperationInput input);
        GetOperationOutput GetOperationById(GetOperationInput input);
    }
}
