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
using App.Caliset.Models.Samples;
using App.Caliset.Samples.Dto;
using App.Caliset.Comments.Dto;
using App.Caliset.Models.Comments;
using App.Caliset.Users.Dto;
using App.Caliset.Authorization.Users;
using App.Caliset.Models.Operations;
using App.Caliset.Operations.Dto;
using App.Caliset.Assignations.Dto;
using App.Caliset.Models.Assignations;
using App.Caliset.Models.UserFile;
using App.Caliset.UserFiles.Dto;
using App.Caliset.HoursRecord.Dto;
using App.Caliset.Models.HoursRecords;

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
                //User
                mapper.CreateMap<UserDtoOperation, User>().ReverseMap();
                mapper.CreateMap<UserDtoUpdate, User>().ReverseMap();
                mapper.CreateMap<UserUpdateRolesDto, User>().ReverseMap();

                //OperationType
                mapper.CreateMap<CreateOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeInput, OperationType>().ReverseMap();
                mapper.CreateMap<GetOperationTypeOutput, OperationType>().ReverseMap();
                mapper.CreateMap<UpdateOperationTypeInput, OperationType>().ReverseMap();

                //OperationState
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

                //Sample
                mapper.CreateMap<CreateSampleInput, Sample>().ReverseMap();
                mapper.CreateMap<GetSampleInput, Sample>().ReverseMap();
                mapper.CreateMap<GetSampleOutput, Sample>().ReverseMap();
                mapper.CreateMap<GetSampleOutputReduced, Sample>().ReverseMap();
                mapper.CreateMap<DeleteSampleInput, Sample>().ReverseMap();

                //Comment
                mapper.CreateMap<CreateCommentInput, Comment>().ReverseMap();
                mapper.CreateMap<GetCommentOutput, Comment>().ReverseMap();
                mapper.CreateMap<GetCommentInput, Comment>().ReverseMap();
                mapper.CreateMap<DeleteCommentInput, Comment>().ReverseMap();
                mapper.CreateMap<UpdateCommentInput, Comment>().ReverseMap();

                //Operation
                mapper.CreateMap<CreateOperationInput, Operation>().ReverseMap();
                mapper.CreateMap<GetOperationOutput, Operation>().ReverseMap();
                mapper.CreateMap<GetOperationInput, Operation>().ReverseMap();
                mapper.CreateMap<DeleteOperationInput, Operation>().ReverseMap();
                mapper.CreateMap<UpdateOperationInput, Operation>().ReverseMap();
                mapper.CreateMap<ReduceOperationOutput, Operation>().ReverseMap();

                //UserFile
                mapper.CreateMap<CreateUserFileInput, UserFile>().ReverseMap();
                mapper.CreateMap<GetUserFileOutput, UserFile>().ReverseMap();

                //Assignation
                mapper.CreateMap<CreateAssignationInput, Assignation>().ReverseMap();
                mapper.CreateMap<GetAssignationOutput, Assignation>().ReverseMap();
                mapper.CreateMap<GetAssignationInput, Assignation>().ReverseMap();
                mapper.CreateMap<DeleteAssignationInput, Assignation>().ReverseMap();

                //Hours Record
                mapper.CreateMap < CreateHoursRecordInput, HourRecord>().ReverseMap();
                mapper.CreateMap<GetHoursRecordOutput, HourRecord>().ReverseMap();
                mapper.CreateMap<UpdateHoursRecordInput, HourRecord>().ReverseMap();

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
