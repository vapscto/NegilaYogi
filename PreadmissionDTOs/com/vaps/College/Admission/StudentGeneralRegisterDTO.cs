using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class StudentGeneralRegisterDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ACQ_Id { get; set; }      

        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public Array check_list { get; set; }
        public Array studentlist { get; set; }
        public string IVRM_CLM_coloumn { get; set; }
        public StudentGeneralRegisterDTO[] TempararyArrayListcoloumn { get; set; }
        public string IVRM_CLG_PAR { get; set; }
        public string column_value { get; set; }
        public string AMST_AadharNo { get; set; }
        public string gender { get; set; }
        public string AMB_BranchName { get; set; }
        public int AMB_Order { get; set; }
        public Array quotalist { get; set; }
        public string Flag { get; set; }

    }
}
