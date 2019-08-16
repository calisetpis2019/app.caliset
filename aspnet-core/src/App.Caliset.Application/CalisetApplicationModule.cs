using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using App.Caliset.Authorization;

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
