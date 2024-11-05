using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_CandidateTypeDTO : CommonParamDTO
    {
        public long HRMCT_Id { get; set; }
        public long HRCL_Id { get; set; }
        public string HRMCT_Name { get; set; }
        public int HRMCT_Order { get; set; }
        public string HRMCT_Desc { get; set; }
        public bool HRMCT_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}