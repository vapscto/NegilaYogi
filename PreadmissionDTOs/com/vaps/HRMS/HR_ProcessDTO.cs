using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_ProcessDTO:CommonDTO
    {

        public long HRPA_Id { get; set; }
        public long MI_Id { get; set; }
    //    public long HRMLN_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public long Id { get; set; }
        public long LogInUserId { get; set; }
        public string HRLP_EmailTo { get; set; }
        public string HRLP_EmailCC { get; set; }
        public string HRPA_TypeFlag { get; set; }
        public long roleId { get; set; }
        public Array gmasterloanList { get; set; }
        public string retrunMsg { get; set; }       

        public Array groupTypedropdownlist { get; set; }

        public Array departmentdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }

        public Array approveid { get; set; }

        public Array privalue { get; set; }
        public Array authData { get; set; }

        public temp_hr_process[] flaghr { get; set; }
        public tempactivites[] tempactivites { get; set; }

        public long UserId { get; set; }
        public long HRPAON_Id { get; set; }
        public int IVRMUL_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public bool HRPAON_FinalFlg { get; set; }

        public bool returnval { get; set; }

        public Array gridlist { get; set; }

        public string IVRMUL_UserName { get; set; }
        public Array griddisplay { get; set; }
        public Array editdata { get; set; }

        public string IVRMSTAUL_UserName { get; set; }

        public string HRMGT_NAME { get; set; }
        public string HRMD_NAME { get; set; }
        public string HRMDES_NAME { get; set; }
        public approvaluser_array[] approvaluser_array { get; set; }
    }
    public class temp_hr_process
    {
        public bool selected { get; set; }

    }
    public class tempactivites
    {
        public string columnName { get; set; }
        public string columnID { get; set; }
    }

    public class approvaluser_array
    {
        public string ApprovalEmpName { get; set; }
        public string hR_PR_NAME { get; set; }
        public long hR_PR_ID { get; set; }
        public long Approval_HRME_Id { get; set; }
        public int ApprovalLevelNo { get; set; }
        public bool ApprovalFinalFlag { get; set; }
    }
}
