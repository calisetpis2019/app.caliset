﻿using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using App.Caliset.Authorization;
using App.Caliset.Models.OperationTypes;
using App.Caliset.OperationTypes.Dto;
using App.Caliset.Clients.Dto;
using App.Caliset.Models.Clients;

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
                mapper.CreateMap<CreateOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeOutput, OperationType>().ReverseMap();
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
