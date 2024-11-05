using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_Member_StatusDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public long CMSMEMSTS_Id { get; set; }
        public string IMFY_FinancialYear { get; set; }
        
        public long CMSMMEM_Id { get; set; }
        public long IMFY_Id { get; set; }
        public decimal CMSMEMSTS_OpeningBalance { get; set; }
        public string CMSMEMSTS_OBCRDRFlg { get; set; }
        public decimal CMSMEMSTS_TotalDR { get; set; }
        public decimal CMSMEMSTS_TotalDRTrans { get; set; }
        public decimal CMSMEMSTS_TotalCRTrans { get; set; }
        public decimal CMSMEMSTS_ClosingBalance { get; set; }
        public string CMSMEMSTS_CBDRDRFlg { get; set; }
        public Array getreport { get; set; }
        public Array finacial { get; set; }
        public string CMSMMEM_MemberFirstName { get; set; }
        public bool CMSMEMSTS_ActiveFlg { get; set; }
        public DateTime? CMSMEMSTS_CreatedDate { get; set; }
        public Array getname { get; set; }
    }
}
