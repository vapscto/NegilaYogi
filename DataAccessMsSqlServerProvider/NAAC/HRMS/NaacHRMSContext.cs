using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.FeedBack;
using DomainModel.Model.HRMS;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.HRMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.HRMS
{
    public class NaacHRMSContext : DbContext
    {
        public NaacHRMSContext(DbContextOptions<NaacHRMSContext> options) : base(options)
        { }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<HR_Employee_OrientationCourseDMO> HR_Employee_OrientationCourseDMO { get; set; }
        public DbSet<HR_Employee_StudentActivitiesDMO> HR_Employee_StudentActivitiesDMO { get; set; }
        public DbSet<HR_Employee_ResearchProjectsDMO> HR_Employee_ResearchProjectsDMO { get; set; }
        public DbSet<HR_Employee_ResearchGuidanceDMO> HR_Employee_ResearchGuidanceDMO { get; set; }
        public DbSet<HR_Employee_BOSBOEDMO> HR_Employee_BOSBOEDMO { get; set; }
        public DbSet<HR_Employee_BOSBOE_CommentsDMO> HR_Employee_BOSBOE_CommentsDMO { get; set; }
        public DbSet<HR_Employee_JournalDMO> HR_Employee_JournalDMO { get; set; }
        public DbSet<HR_Employee_ConferenceDMO> HR_Employee_ConferenceDMO { get; set; }
        public DbSet<HR_Employee_BookDMO> HR_Employee_BookDMO { get; set; }
        public DbSet<HR_Employee_BookChapterDMO> HR_Employee_BookChapterDMO { get; set; }
        public DbSet<HR_Employee_CommitteeDMO> HR_Employee_CommitteeDMO { get; set; }
        public DbSet<HR_Employee_OtherDetailsDMO> HR_Employee_OtherDetailsDMO { get; set; }
        public DbSet<HR_Employee_DevActivitiesDMO> HR_Employee_DevActivitiesDMO { get; set; }
        public DbSet<HR_MasterExam_GroupADMO> HR_MasterExam_GroupADMO { get; set; }
        public DbSet<HR_MasterExam_GroupBDMO> HR_MasterExam_GroupBDMO { get; set; }
        public DbSet<HR_Employee_GroupAExamDMO> HR_Employee_GroupAExamDMO { get; set; }
        public DbSet<HR_Employee_GroupBExamDMO> HR_Employee_GroupBExamDMO { get; set; }
        public DbSet<NAACACCommitteeDMO> NAAC_AC_CommitteeDMO { get; set; }
        public DbSet<NAACACCommitteeMembersDMO> NAACACCommitteeMembersDMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Master_Employee_Documents> Master_Employee_Documents { get; set; }
        public DbSet<Master_Employee_Qulaification> Master_Employee_Qulaification { get; set; }
        public DbSet<HR_Employee_ExamDutyDMO> HR_Employee_ExamDutyDMO { get; set; }
        public DbSet<HR_Employee_BOSBOE_FilesDMO> HR_Employee_BOSBOE_FilesDMO { get; set; }
        public DbSet<HR_Employee_BOSBOE_File_CommentsDMO> HR_Employee_BOSBOE_File_CommentsDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<FeedBackMasterTypeDMO>().HasKey(m => m.FMTY_Id); 
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<FeedBackMasterTypeDMO>();       

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
