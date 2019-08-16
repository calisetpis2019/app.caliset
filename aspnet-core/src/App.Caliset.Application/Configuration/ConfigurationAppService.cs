using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using App.Caliset.Configuration.Dto;

namespace App.Caliset.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : CalisetAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
