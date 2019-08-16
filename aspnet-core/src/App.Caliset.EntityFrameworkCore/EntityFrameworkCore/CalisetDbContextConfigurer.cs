using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace App.Caliset.EntityFrameworkCore
{
    public static class CalisetDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CalisetDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CalisetDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
