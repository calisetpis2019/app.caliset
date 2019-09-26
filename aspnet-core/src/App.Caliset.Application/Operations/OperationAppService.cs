using Abp.Application.Services;
using App.Caliset.Auxiliares;
using App.Caliset.Models.Clients;
using App.Caliset.Models.Locations;
using App.Caliset.Models.Operations;
using App.Caliset.Models.OperationStates;
using App.Caliset.Models.OperationTypes;
using App.Caliset.Operations.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Operations
{
    public class OperationAppService : ApplicationService, IOperationAppService
    {


        private readonly IOperationManager _operationManager;
        private readonly IOperationStateManager _operationStateManager;
        private readonly ILocationManager _locationManager;
        private readonly IClientManager _clientManager;
        private readonly IOperationTypeManager _operationTypeManager;

        private readonly IFiltros _filtroManager;

        public OperationAppService(IOperationManager operationManager
                                    ,IOperationStateManager operationStateManager
                                    ,ILocationManager locationManager
                                    ,IClientManager clientManager
                                    ,IOperationTypeManager operationTypeManager
                                    ,IFiltros filtroManager
            )
        {
            _operationManager = operationManager;
            _operationStateManager = operationStateManager;
            _locationManager = locationManager;
            _clientManager = clientManager;
            _operationTypeManager = operationTypeManager;
            _filtroManager = filtroManager;
        }



        public async Task Create(CreateOperationInput input)
        {
            // input.Inspectors = null;
            //  input.Comments = null;
            Operation Oper = new Operation
            {
                OperationState = _operationStateManager.GetOperationStateById(input.OperationState),
                Location = _locationManager.GetLocationById(input.IdLocation),
                Nominador = _clientManager.GetClientById(input.Nominador),
                Cargador = _clientManager.GetClientById(input.Cargador),
                OperationType = _operationTypeManager.GetOperationTypeById(input.OperationType),
                Date = input.Date,
                Commodity = input.Commodity,
                Package = input.Package,
                ShipName = input.ShipName,
                Destiny = input.Destiny,
                Line = input.Line,
                BookingNumber = input.BookingNumber,
          
    };

            await _operationManager.Create(Oper);
        }

        public void Delete(DeleteOperationInput input)
        {
            _operationManager.Delete(input.Id);
        }

        public IEnumerable<GetOperationOutput> GetAll()
        {
            var getAll = _operationManager.GetAll().ToList();
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(getAll);
            return output;
        }

        public IEnumerable<GetOperationOutput> GetAllByState(String State)
        {
            var getAll = _operationManager.GetAll().ToList();
            List<Operation> aux = _filtroManager.FOperationsState(getAll, State);
            List<GetOperationOutput> output = ObjectMapper.Map<List<GetOperationOutput>>(aux);
            return output;
        }

        public GetOperationOutput GetOperationById(GetOperationInput input)
        {
            var Oper = _operationManager.GetAll().FirstOrDefault(x => x.Id == input.Id);
            var ret = ObjectMapper.Map<GetOperationOutput>(Oper);

            return ret;
        }

        public void Update(UpdateOperationInput input)
        {
            var Oper = _operationManager.GetOperationById(input.Id);

            Oper.Location = _locationManager.GetLocationById(input.IdLocation);
            Oper.Nominador = _clientManager.GetClientById(input.Nominador);
            Oper.Cargador = _clientManager.GetClientById(input.Cargador);
            Oper.OperationType = _operationTypeManager.GetOperationTypeById(input.OperationType);
            Oper.Date = input.Date;
            Oper.Commodity = input.Commodity;
            Oper.Package = input.Package;
            Oper.ShipName = input.ShipName;
            Oper.Destiny = input.Destiny;
            Oper.Line = input.Line;
            Oper.BookingNumber = input.BookingNumber;

            _operationManager.Update(Oper);
        }
    }
}
