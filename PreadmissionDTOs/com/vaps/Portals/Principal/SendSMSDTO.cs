using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class SendSMSDTO : CommonParamDTO
    {
        //public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public Array employeedropdown { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long ASMCL_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array studentlist { get; set; }
        public string Mobno { get; set; }
        public string mes { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public long AMCST_Id { get; set; }
        public Array StaffName { get; set; }
        public long roleId { get; set; }
        public string selectedRadiobtn { get; set; }
        public Array currentYear { get; set; }
        public Array employe { get; set; }
        public Array stafflist { get; set; }
        public string studentName { get; set; }
        public int studentCount { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public string multipledep { get; set; }
        public string multipledes { get; set; }
        
        public SendSMSDTO[] studentlistdto { get; set; }
        public string SmsMailText { get; set; }
        public string radiotype { get; set; }
        public string smsStatus { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string emailStatus { get; set; }
        public string hrm_email { get; set; }

        //=====CLG_PrincipleSMS_Send======//
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }




    }
}


