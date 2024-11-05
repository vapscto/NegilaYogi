using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
  public  class CMS_Master_MemberMobileNoDTO
    {
        public long CMSMMEMMN_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public long CMSMMEMMN_MobileNo { get; set; }
        public bool CMSMMEMMN_DeFaultFlag { get; set; }
        public bool CMSMMEMMN_ActiveFlg { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
    }
}
