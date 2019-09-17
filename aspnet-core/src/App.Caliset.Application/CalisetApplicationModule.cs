using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using App.Caliset.Authorization;
using App.Caliset.Models.OperationTypes;
using App.Caliset.OperationTypes.Dto;
using App.Caliset.Clients.Dto;
using App.Caliset.Models.Clients;
using App.Caliset.OperationStates.Dto;
using App.Caliset.Models.OperationStates;
using App.Caliset.Locations.Dto;
using App.Caliset.Models.Locations;

namespace App.Caliset
{
    [DependsOn(
        typeof(CalisetCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class CalisetApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<CalisetAuthorizationProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //OperationType
                mapper.CreateMap<CreateOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeOutput, OperationType>().ReverseMap();
                mapper.CreateMap<UpdateOperationTypeInput, OperationType>().ReverseMap();

                //OperationState
                mapper.CreateMap<CreateOperationStateInput, OperationState>().ReverseMap();
                mapper.CreateMap<GetOperationStateInput, OperationState>().ReverseMap();
                mapper.CreateMap<GetOperationStateOutput, OperationState>().ReverseMap();
                mapper.CreateMap<UpdateOperationStateInput, OperationState>().ReverseMap();

                //Location
                mapper.CreateMap<CreateLocationInput, Location>().ReverseMap();
                mapper.CreateMap<GetLocationInput, Location>().ReverseMap();
                mapper.CreateMap<GetLocationOutput, Location>().ReverseMap();
                mapper.CreateMap<UpdateLocationInput, Location>().ReverseMap();

                //Client
                mapper.CreateMap<CreateClientInput, Client>().ReverseMap();
                mapper.CreateMap<GetClientInput, Client>().ReverseMap();
                mapper.CreateMap<GetClientOutput, Client>().ReverseMap();
                mapper.CreateMap<ClientDto, Client>().ReverseMap();
                mapper.CreateMap<UpdateClientInput, Client>().ReverseMap();
            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(CalisetApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
