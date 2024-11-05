using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_Qual_SubjectDTO : CommonParamDTO
    {
        public long HRMQS_Id { get; set; }
        public long HRMQSC_Id { get; set; }
        public string HRMQS_Name { get; set; }
        public bool HRMQS_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}