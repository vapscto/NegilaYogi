using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement
{
    public class VisitorsManagementContext : DbContext
    {
        public VisitorsManagementContext(DbContextOptions<VisitorsManagementContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        public VisitorsManagementContext()
        {
        }
        public DbSet<Adm_M_Student> admissionStduent { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> admissionyearstudent { get; set; }
        public DbSet<School_M_Class> admissionClass { get; set; }
        public DbSet<School_M_Section> masterSection { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet <HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<AddVisitorsDMO> AddVisitorsDMO { get; set; }
        public DbSet<InwardDMO> InwardDMO { get; set; }
        public DbSet<OutwardDMO> OutwardDMO { get; set; }
        public DbSet<GatePassDMO> GatePassDMO { get; set; }
     
        public DbSet<Multiple_Mobile_DMO> Emp_MobileNo { get; set; }       
        public DbSet<Visitor_Management_Master_Location_DMO> Visitor_Management_Master_Location_DMO { get; set; }
        public DbSet<FO_Inward_DMO> FO_Inward_DMO { get; set; }
        public DbSet<Visitor_Management_Appointment_DMO> Visitor_Management_Appointment_DMO { get; set; }
        public DbSet<Visitor_Management_Appointment_VisitorsDMO> Visitor_Management_Appointment_VisitorsDMO { get; set; }
        public DbSet<Visitor_Management_Appointment_FilesDMO> Visitor_Management_Appointment_FilesDMO { get; set; }
        public DbSet<Visitor_Management_Visitor_Appointment_DMO> Visitor_Management_Visitor_Appointment_DMO { get; set; }
        public DbSet<Visitor_Management_MasterVisitor_DMO> Visitor_Management_MasterVisitor_DMO { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }
        public DbSet<FO_Outward_DMO> FO_Outward_DMO { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<Gate_Pass_Student_DMO> Gate_Pass_Student_DMO { get; set; }
        public DbSet<Gate_Pass_Staff_DMO> Gate_Pass_Staff_DMO { get; set; }
        public DbSet<LateInStudent_DMO> LateInStudent_DMO { get; set; }
        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Institution> Institute { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<HR_Master_Floor_DMO> HR_Master_Floor_DMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }
        public DbSet<VehicleRouteDMo> VehicleRouteDMo { get; set; }
        public DbSet<VehicleDriver> VehicleDriver { get; set; }
        public DbSet<MasterDriverDMO> MasterDriverDMO { get; set; }
        public DbSet<Multiple_VisitorDMO> Multiple_VisitorDMO { get; set; }
        public DbSet<Visitor_Management_Visitor_ToMeetDMO> Visitor_Management_Visitor_ToMeetDMO { get; set; }
        public DbSet<VM_Master_Visitor_FileDMO> VM_Master_Visitor_FileDMO { get; set; }
        public DbSet<ApplicationUserRole> appUserRole { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }     
        public DbSet<SMS_Sent_Details> SMS_Sent_Details { get; set; }
        public DbSet<SMSMasterApprovalDMO> SMSMasterApprovalDMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           // base.OnModelCreating(builder);
            //builder.Entity<AddVisitorsDMO>().HasKey(m => m.AMVM_Id);
            builder.Entity<InwardDMO>().HasKey(m => m.IW_Id);
            builder.Entity<OutwardDMO>().HasKey(m => m.OW_Id);

        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            //updateUpdatedProperty<AddVisitorsDMO>();
            updateUpdatedProperty<InwardDMO>();
            updateUpdatedProperty<OutwardDMO>();

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
