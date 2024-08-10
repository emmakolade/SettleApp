using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Settle_App.Models.Domain;



namespace Settle_App.Data
{
    public class SettleAppDBContext(DbContextOptions<SettleAppDBContext> dbContextOptions) : IdentityDbContext<SettleAppUser>(dbContextOptions)
    {
        public DbSet<Register> Register { get; set; }
        public DbSet<Wallet> Wallet { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("SettleApp");
            
            builder.Entity<Register>()
                .Property(p => p.PhoneNumber)
                .HasMaxLength(15);
            
            builder.Entity<Wallet>()
            .Property(w=>w.Balance)
            .HasColumnType("decimal(18.2)");

        }
    }
}

// namespace Settle_App.Data
// {
//     public class SettleAppDBContext(DbContextOptions<SettleAppDBContext> dbContextOptions) : DbContext(dbContextOptions)
//     {
//         public DbSet<Wallet> Wallet { get; set; }
        
       
//     }
// }
