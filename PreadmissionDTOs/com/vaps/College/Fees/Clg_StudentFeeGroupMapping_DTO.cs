using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class Clg_StudentFeeGroupMapping_DTO
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

        public Clg_StudentFeeGroupMapping_DTO[] unselectedlist { get; set; }
        public bool studchecked { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }
        public bool checkedinstallmentlst { get; set; }
        public string returnval { get; set; }
        public Array editdatalist { get; set; }
        public Array schemeType { get; set; }
        public long ACST_Id { get; set; }

        public Array fillyear { get; set; }
        public Array institutionList { get; set; }
        public Array classList { get; set; }
        public Array groupNameList { get; set; }
        public Array headNameList { get; set; }
        public Array installmentList { get; set; }
        public Array termList { get; set; }
        public Array subMerchantIdList { get; set; }
        public Array feeonlinepaymentmappingList { get; set; }
        public int count { get; set; }
        public long FOPM_Id { get; set; }
        public Fee_OnlinePayment_MappingDTO[] selectedheadList { get; set; }
        public string Isduplicate { get; set; }
        public int headlistcount { get; set; }
        public int installmentlistcount { get; set; }
        public Array editdata { get; set; }
        public long fmg_id { get; set; }
        public long fpgd_id { get; set; }
        public long fti_id { get; set; }
        public long CFOPM_Id { get; set; }
        public string InstitutionName { get; set; }
        public string FPGD_SubMerchantId { get; set; }

        public Array stafflist { get; set; }
        public Array saved_stafflist { get; set; }

        public Array grid_stafflist { get; set; }

        public long FMSTGH_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }

        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public string radioval { get; set; }
        public Array oth_studentlist { get; set; }

        public Array saved_oth_studentlist { get; set; }
        public Array grid_oth_studentlist { get; set; }

        public long FMOSTGH_Id { get; set; }
        public long FMOST_Id { get; set; }
        public string FMOST_StudentName { get; set; }
        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }

        public bool disableins { get; set; }
        public bool checkedgrplstedit { get; set; }

        public bool checkedheadlstedit { get; set; }

        public bool checkedinstallmentlstedit { get; set; }

        public Array academicdrp { get; set; }

        public Array configsetting { get; set; }
        public Temp_Staff_DTO[] staff_list { get; set; }

        public Array alldatathird { get; set; }

        public long FMSG_Id { get; set; }

        public long FSS_PaidAmount { get; set; }

        public Fee_Master_OtherStudentsDTO[] student_list { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }



    }
}
