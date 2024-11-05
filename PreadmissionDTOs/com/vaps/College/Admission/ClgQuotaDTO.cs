using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgQuotaDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        //------------------------Master Quota

        public long ACQ_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string ACQ_QuotaCode { get; set; }
        public string ACQ_QuotaInfo { get; set; }
        public bool ACQ_ActiveFlg { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }

        //-----------------------------Quota category
        public long ACQC_Id { get; set; }      
        public string ACQC_CategoryName { get; set; }
        public string ACQC_CategoryCode { get; set; }
        public string ACQC_CategoryInfo { get; set; }
        public bool ACQC_ActiveFlg { get; set; }
        public Array getdetails1 { get; set; }
        public Array editdetails1 { get; set; }

        //-----------------------------Quota category Mapping
        public long ACQCM_Id { get; set; }    
        public bool ACQCM_ActiveFlg { get; set; }
        public ClgQuotaDTO[] QuotaClist { get; set; }
        public Array getdetails2 { get; set; }
    }
}
