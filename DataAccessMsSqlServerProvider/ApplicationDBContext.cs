using DomainModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{

    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)

        {
            Database.SetCommandTimeout(300000);
        }

        //public DbSet<ApplicationUser> applicationUser { get; set; }

        public DbSet<ApplicationUser> applicationUser { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> Subject { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var user = modelBuilder.Entity<IdentityUser>().ToTable("AppUser", "Users"); //Specify our our own table names instead of the defaults

            //user.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId);
            //user.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId);
            //user.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId);
            //user.Property(u => u.UserName).IsRequired();

            var applicationUser = modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser"); //Specify our our own table names instead of the defaults

            //applicationUser.Property(au => au.Id).HasColumnName("Id");
            //applicationUser.Property(au => au.NumericId).HasColumnName("NumericId");
            applicationUser.Property(au => au.UserName).HasMaxLength(50).HasColumnName("UserName").HasColumnType("VARCHAR(4000)");
           
            //applicationUser.Property(au => au.Email).HasColumnName("EmailAddress").HasMaxLength(254).IsRequired();
            

            var role = modelBuilder.Entity<ApplicationRole>().ToTable("ApplicationRole");

            //role.Property(ir => ir.Id).HasColumnName("Id");
            //role.Property(ir => ir.Name).HasColumnName("Name");

            var claim = modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("ApplicationUserClaim");

            //claim.Property(iuc => iuc.Id).HasColumnName("Id");
            //claim.Property(iuc => iuc.ClaimType).HasColumnName("ClaimType");
            //claim.Property(iuc => iuc.ClaimValue).HasColumnName("ClaimValue");
            //claim.Property(iuc => iuc.UserId).HasColumnName("UserId");

            // var login = modelBuilder.Entity<ApplicationUserLogins>().ToTable("UserLogin"); //Used for third party OAuth providers


            var login = modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("ApplicationUserLogin");

            //login.Property(iul => iul.UserId).HasColumnName("UserId");
            //login.Property(iul => iul.LoginProvider).HasColumnName("LoginProvider");
            //login.Property(iul => iul.ProviderKey).HasColumnName("ProviderKey");

            var userRole = modelBuilder.Entity<IdentityUserRole<int>>().ToTable("ApplicationUserRole");

            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("ApplicationRoleClaims");

            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("ApplicationUserToken");

            //userRole.Property(ur => ur.UserId).HasColumnName("UserId");
            //userRole.Property(ur => ur.RoleId).HasColumnName("RoleId");


        }
    }
}




