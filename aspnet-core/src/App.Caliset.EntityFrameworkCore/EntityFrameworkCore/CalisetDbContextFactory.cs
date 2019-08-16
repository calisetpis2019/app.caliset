using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using App.Caliset.Configuration;
using App.Caliset.Web;

namespace App.Caliset.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CalisetDbContextFactory : IDesignTimeDbContextFactory<CalisetDbContext>
    {
        public CalisetDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CalisetDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CalisetDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CalisetConsts.ConnectionStringName));

            return new CalisetDbContext(builder.Options);
        }
    }
}
