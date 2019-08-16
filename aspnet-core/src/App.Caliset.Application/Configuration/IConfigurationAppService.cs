using System.Threading.Tasks;
using App.Caliset.Configuration.Dto;

namespace App.Caliset.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
