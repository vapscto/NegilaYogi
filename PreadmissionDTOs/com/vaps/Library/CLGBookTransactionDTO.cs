using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class CLGBookTransactionDTO: CommonParamDTO
    {
        public string AMCST_emailId { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public long Book_Trans_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public DateTime? Issue_Date { get; set; }
        public DateTime? Due_Date { get; set; }
        public DateTime Return_Date { get; set; }
        public decimal Fine_Amount { get; set; }
        public decimal Waived_Amount { get; set; }
        public int Renewal_Counter { get; set; }
        public string Book_Trans_Status { get; set; }
        public long FODM_Id { get; set; }
        public long Guest_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public string AMST_FirstName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public Array studentlist { get; set; }
        public Array configdata { get; set; }

        public Array booktitle { get; set; }
        public Array alldata { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_Photoname { get; set; }
        public string ASMC_SectionName { get; set; }
        public long AMAY_RollNo { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_ClassNo { get; set; }
        public decimal LMB_Price { get; set; }
        public string LMB_VolNo { get; set; }
        public DateTime LMB_EntryDate { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int LBTR_Renewalcounter { get; set; }
        public Array studentdata { get; set; }
    
        public long ASMCL_Id { get; set; }
        public long ASYST_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array bookdetails { get; set; }
        public long LMB_Id { get; set; }
        public long LMBA_Id { get; set; }
        public long LMS_Id { get; set; }
        public long LMD_Id { get; set; }
        public long HRME_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LML_Id { get; set; }
        public long Donor_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long Rack_Id { get; set; }
        public Array getalldata { get; set; }
        public Array editlist { get; set; }
        public long Max_Issue_Items { get; set; }
        public long Max_Issue_Days { get; set; }
        public long Max_No_Renewals { get; set; }
        public string issuertype { get; set; }
        public string booktype { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public bool renew { get; set; }
        public Array stafftlist { get; set; }
        public Array departmentlist { get; set; }
        public Array circularparamdetails { get; set; }
        public Array selctstaffdata { get; set; }
        public long? HRMGT_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LMD_DepartmentCode { get; set; }
        public string LBTR_GuestName { get; set; }
        public long LBTR_GuestContactNo { get; set; }
        public string LBTR_GuestEmailId { get; set; }
        public string searchfilter { get; set; }
        public string bookcat_type { get; set; }

        public long HRMD_Id { get; set; }
        public int maxitem { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Int32? HRMD_Order { get; set; }
        public string maxmsg { get; set; }

        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public long HRMDES_Id { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public Array yearlist { get; set; }
        public Array currentYear { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array liblist { get; set; }
        public Array msterliblist { get; set; }
        public Array msterliblist1 { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public int studentCount { get; set; }
        public long IVRMUL_Id { get; set; }
        public string msg { get; set; }
        public string clamsg { get; set; }

        public long LMAL_Id { get; set; }
        public string  app { get; set; }

        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set; }

        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public string AMB_BranchType { get; set; }

        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public int AMB_Order { get; set; }
        public long AMSE_Id { get; set; }
       
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMInfo { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }

        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
        public bool ACMS_ActiveFlag { get; set; }
        public int ACMS_MaxCapacity { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string searctype { get; set; }



        //==============================Report data
        public Array lib_list { get; set; }
        public Array reportlist { get; set; }
        public string AGType { get; set; }
        public string Type { get; set; }
        public string TrnType { get; set; }
        public string IssueFromDate { get; set; }
        public string IssueToDate { get; set; }
        public string DueFromdate { get; set; }
        public string DueTodate { get; set; }

        public CLGBookTransactionDTO[] selectedcourse { get; set; }
        public CLGBookTransactionDTO[] selectedbrnchlst { get; set; }
        public CLGBookTransactionDTO[] selectedsemlst { get; set; }
      

    }
}
