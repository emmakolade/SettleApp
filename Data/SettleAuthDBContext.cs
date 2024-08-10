using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Settle_App.Models.Domain;

namespace Settle_App.Data
{
    public class SettleAuthDBContext(DbContextOptions<SettleAuthDBContext> authDBContextOptions) : IdentityDbContext<SettleAppUser>(authDBContextOptions)
    {
        public DbSet<Register> Register { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("SettleApp");
      
            builder.Entity<Register>()
            .Property(p=> p.PhoneNumber)
            .HasMaxLength(15);

            // Consider using projections for performance optimization (optional)
            // modelBuilder.Entity<ApplicationUser>()
            //     .HasSelectQuery(u => new { u.Id, u.UserName, u.Email, u.PhoneNumber }); // Select only required properties

        }

    }
}
