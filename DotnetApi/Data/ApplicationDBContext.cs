
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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