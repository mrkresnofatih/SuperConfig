using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperConfig.EntityFramework
{
    public class PgSuperConfigDbContext : SuperConfigDbContext
    {
        public PgSuperConfigDbContext(DbContextOptions<PgSuperConfigDbContext> options) : base(options)
        {
        }
    }
}
