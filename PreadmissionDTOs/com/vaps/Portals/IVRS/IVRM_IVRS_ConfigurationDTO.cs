using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class IVRM_IVRS_ConfigurationDTO
    {
        public long IIVRSC_Id { get; set; }
        public string IIVRSC_VirtualNo { get; set; }
        public long IIVRSC_MI_Id { get; set; }
        public string IIVRSC_SchoolName { get; set; }
        public string IIVRSC_URL { get; set; }
        public string IIVRSC_VFORTTSFlg { get; set; }
        public string IVRS_MOBILE_URL { get; set; }
        public string IVRS_UPDATE_URL { get; set; }
        public long IIVRSC_PerMonthCall { get; set; }
        public decimal IIVRSC_CallCharge { get; set; }
        public bool IIVRSC_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string ivrM_Month_Name { get; set; }
        public IVRS_Call_StatusDTO[] IVRS_Call_StatusDTO { set; get; }
        public long ASMAY_ID { get; set; }
        //============
        public long MI_Id { get; set; }
        public string PNSD_Header_Flg { get; set; }
        public string PNSD_HeaderName { get; set; }
        public string PNSD_SMSMessage { get; set; }
        public long userid { get; set; }
      public Array notification_stu_stf_list { get; set; }
        public Array maindata_grid { get; set; }
    }
}
