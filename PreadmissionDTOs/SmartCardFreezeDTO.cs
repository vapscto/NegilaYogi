using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SmartCardFreezeDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int yearid { get; set; }
        public string MI_SchoolCollegeFlag { get; set; }
        public int studentCount { get; set; }
        public Array yeardropDown { get; set; }
        public Array selctstaffdata { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long asmayid { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public Array yearfromTo { get; set; }  
        public Array yearlist { get; set; }  
        public Array currentYear { get; set; }  
        public Array studentlist { get; set; }
        public Array studentdetails { get; set; }
        public Array classlist { get; set; }  
        public Array sectionlist { get; set; }  
        public SMSReportDTO[] sms_listarray { get; set; }
        public SMSReportDTO[] mial_listarray { get; set; }
        public Array sms_mial_listarray { get; set; }
        public Array meritlistdata { get; set; }
        public string HRME_Photo { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array stafftlist { get; set; }
        public Array SCFLAGLIST { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public long HRME_Id { get; set; }
        public string studentFName { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string studentMName { get; set; }
        public string studentLName { get; set; }
        public long pasr_id { get; set; }
        public decimal obtainedmarks { get; set; }
        public string passfail_flag { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string Name { get; set; }
        public string smscount { get; set; }
        public string emailcount { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
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
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }


    }
}
