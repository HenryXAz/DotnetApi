
using DotnetApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotnetApi.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser> 
    { 
        public ApplicationDBContext(DbContextOptions dbContextOptions)
         : base(dbContextOptions)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => {
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
            });
            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<Stock> Stocks {get; set;}
        public DbSet<Comment> Comments {get; set;}
        public DbSet<Portfolio> Portfolios {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new {p.UserId, p.StockId}));

            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = [
                new(){
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new(){
                    Name = "User",
                    NormalizedName = "USER"
                },
                
            ];

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}