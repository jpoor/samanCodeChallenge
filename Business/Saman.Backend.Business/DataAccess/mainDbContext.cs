using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Saman.Backend.Business.DataAccess
{
    public class mainDbContext : IdentityDbContext<Entity.Authentication.User_dBo>
    {
        public mainDbContext(DbContextOptions<mainDbContext> options) : base(options) { }

        public DbSet<Entity.Category.Category_dBo> Categorys { get; set; }
        public DbSet<Entity.Product.Product_dBo> Products { get; set; }
        public DbSet<Entity.Log.Log_dBo> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignore the specific warning
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Normal Casecading
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            // Deactive casecade deleting
            foreach (var fk in cascadeFKs) { fk.DeleteBehavior = DeleteBehavior.Restrict; }

            // Generate model
            base.OnModelCreating(modelBuilder);

            // Custome configs
            modelBuilder.ApplyConfiguration(new Entity.Authentication.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Entity.Authentication.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new Entity.Authentication.UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new Entity.Category.CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new Entity.Product.ProductConfiguration());
            modelBuilder.ApplyConfiguration(new Entity.Log.LogConfiguration());
        }
    }
}
