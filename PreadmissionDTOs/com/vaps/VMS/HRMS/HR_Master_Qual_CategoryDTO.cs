using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_Qual_CategoryDTO : CommonParamDTO
    {
        public long HRMQC_Id { get; set; }
        public string HRMQC_Name { get; set; }
        public int HRMQC_Order { get; set; }
        public bool HRMQC_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}