using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class LIB_Master_Library_DTO : CommonParamDTO
    {

        public long LMAL_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAL_LibraryName { get; set; }
        public bool LMAL_ActiveFlag { get; set; }
        public long LUL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool LUL_ActiveFlg { get; set; }
        public long LLC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool LLC_ActiveFlg { get; set; }

        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array clsscatglist { get; set; }
        public Array alldata { get; set; }
        public Array mappedclass { get; set; }
        public Array listclassdetails { get; set; }
        public string message { get; set; }
        public string MI_SchoolCollegeFlag { get; set; }
        public Array stafflist { get; set; }
        public long UserId { get; set; }
        public Array librylist { get; set; }
        public Array liballdata { get; set; }
        public Array classlist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array clssslist { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public Array editlist { get; set; }
        public Array role { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public long IVRMRT_Id { get; set; }
        public Array clsdata { get; set; }

        public LIB_Master_Library_DTO[] classlst { get; set; }


    }
}
