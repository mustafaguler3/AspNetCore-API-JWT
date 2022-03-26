using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Entities;
using UdemyAuthServer.Data.Configuration;

namespace UdemyAuthServer.Data.Context
{
    public class AppDbContext : IdentityDbContext<UserApp,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }
        
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);//projeyi tarayıp hepsini enjecte edecek

            //builder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
