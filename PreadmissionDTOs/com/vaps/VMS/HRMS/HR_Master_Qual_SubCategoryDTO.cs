using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_Qual_SubCategoryDTO : CommonParamDTO
    {
        public long HRMQSC_Id { get; set; }
        public long HRMQC_Id { get; set; }
        public string HRMC_SubCategoryName { get; set; }
        public bool HRMC_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}