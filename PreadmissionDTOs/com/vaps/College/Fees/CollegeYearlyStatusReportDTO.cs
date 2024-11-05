using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class CollegeYearlyStatusReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public long ACAYC_Id { get; set; }
        public long FCMSGHI_Id { get; set; }
        public Array yearlist { get; set; }
        public long ACAYCB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public int AMCO_Order { get; set; }
        public string AMCO_CourseName { get; set; }
        public Array courselist { get; set; }
        public long AMB_Id { get; set; }
        public int AMB_Order { get; set; }
        public string AMB_BranchName { get; set; }
        public Array branchlist { get; set; }
        public long AMSE_Id { get; set; }
        public long FCMSGH_Id { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array semesterlist { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_RegistrationNo { get; set; }
      
        public long ACYST_RollNo { get; set; }
        public Array studentrReport { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }
        public Array studentlist { get; set; }
        public long AMCST_Id { get; set; }
        public long user_id { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public Array Studentreport { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public long FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        public Array fillmastergroup { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillinstallment { get; set; }
        public Clg_StudentFeeGroupMapping_DTO[] studentdata { get; set; }
        public Clg_StudentFeeGroupMapping_DTO[] savegrplst { get; set; }
        public Clg_StudentFeeGroupMapping_DTO[] saveheadlst { get; set; }
        public Clg_StudentFeeGroupMapping_DTO[] saveftilst { get; set; }
        public bool studchecked { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }
        public bool checkedinstallmentlst { get; set; }
        public string returnval { get; set; }
        public Array editdatalist { get; set; }
        public Array feedetails { get; set; }
    }
}
