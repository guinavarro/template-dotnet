using Microsoft.EntityFrameworkCore;

namespace Template.Infrastructure
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
        {

        }

        #region DbSet
        #endregion
    }
}
