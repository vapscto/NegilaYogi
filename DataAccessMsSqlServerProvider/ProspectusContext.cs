using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider
{
    public class ProspectusContext : DbContext
    {
        public ProspectusContext(DbContextOptions<ProspectusContext> options) : base(options)
        { }
        public DbSet<Prospectus> prospectus { get; set; }

        public DbSet<FeePaymentDetailsDMO> feeypay { get; set; }

        public DbSet<Country> country { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; } 
        public DbSet<MasterConfiguration> MasterConfiguration { get; set; }
        public DbSet<MasterAcademic> year { get; set; }
        public DbSet<AdmissionClass> AdmClass { get; set; }
        public DbSet<MasterSource> source { get; set; }
        public DbSet<MasterReference> reference { get; set; }
        public DbSet<Enquiry> enquiry { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }

        public DbSet<Prospepaymentamount> Prospepaymentamount { get; set; }
        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_DetailsDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Prospectus>().HasKey(m => m.PASP_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Prospectus>();

            return base.SaveChanges();
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
