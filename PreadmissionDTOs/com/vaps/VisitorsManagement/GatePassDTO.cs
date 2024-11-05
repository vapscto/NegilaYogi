using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class GatePassDTO : CommonParamDTO
    {
        public long AGPH_Id { get; set; }
        public long MI_Id { get; set; }
        public string AGPH_PassType { get; set; }
        public long AGPH_Idno { get; set; }
        public string AGPH_Dategiven { get; set; }
        public string AGPH_Remark { get; set; }
        public Array studentlist { get; set; }
        public Array employeelist { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string returnVal { get; set; }
        public string radiotype { get; set; }
        public Array student { get; set; }
        public long? ASMCL_Id { get; set; }
        public long AMST_MobileNo { get; set; }
        public string Address { get; set; }
        public Array employ { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string date { get; set; }
        public string remarks { get; set; }
        public Array institution { get; set; }
        public Array studDetails { get; set; }
        public DateTime datetime { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ASMAY_Id { get; set; }
    }
}
