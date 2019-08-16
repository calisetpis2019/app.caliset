using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using App.Caliset.Configuration;

namespace App.Caliset.Web.Host.Startup
{
    [DependsOn(
       typeof(CalisetWebCoreModule))]
    public class CalisetWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public CalisetWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CalisetWebHostModule).GetAssembly());
        }
    }
}
