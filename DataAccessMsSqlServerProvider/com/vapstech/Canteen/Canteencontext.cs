using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Canteen;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.PDA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.Canteen
{
   public class Canteencontext : DbContext
    {

        public Canteencontext(DbContextOptions<Canteencontext> options) : base(options) { Database.SetCommandTimeout(30000000); }

        public DbSet<Institution> Institute { get; set; }
        public DbSet<FooditeamDMO> FooditeamDMO { get; set; }
        public DbSet<FoodMasterCategoryDMO> FoodMasterCategoryDMO { get; set; }
        public DbSet<FooditemtaxDMO> FooditemtaxDMO { get; set; }
        public DbSet<Adm_M_Student_WalletPINDMO> Adm_M_Student_WalletPINDMO { get; set; }
        public DbSet<INV_Master_TaxDMO> INV_Master_TaxDMO { get; set; }
        public DbSet<FooditemimageDMO> FooditemimageDMO { get; set; }
        public DbSet<FoodtransactionDMO> FoodtransactionDMO { get; set; }
        public DbSet<CM_Transaction_TaxDMO> CM_Transaction_TaxDMO { get; set; }
        public DbSet<CM_Transaction_PaymentModeDMO> CM_Transaction_PaymentModeDMO { get; set; }
        public DbSet<CM_Transaction_ItemsDMO> CM_Transaction_ItemsDMO { get; set; }

        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<IVRM_ModeOfPayment> Adm_Master_College_StudentDMO { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }
        public DbSet<PDA_StatusDMO> PDA_StatusDMO { get; set; }
       






        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);


        }
    }

}
